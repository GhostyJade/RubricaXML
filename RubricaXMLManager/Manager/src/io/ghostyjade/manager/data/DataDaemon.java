package io.ghostyjade.manager.data;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.List;

import io.ghostyjade.manager.addressbook.Contact;
import io.ghostyjade.manager.file.FileManager;

public class DataDaemon implements Runnable {

	private Thread daemonThread;
	private volatile boolean listening = false;
	private ServerSocket socket;
	private Data data;
	private Socket clientSocket;
	private BufferedReader in;
	private DataOutputStream out;

	public DataDaemon(String address, int port) {
		try {
			socket = new ServerSocket(port);
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

	@Override
	public void run() {
		System.out.println("Server started");
		makeConnection();
		try {
			in = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));
			out = new DataOutputStream(clientSocket.getOutputStream());
		} catch (IOException e1) {
			e1.printStackTrace();
		}

		while (listening) {
			try {
				processPacket(in.readLine());
			} catch (IOException e) {
				e.printStackTrace();
			}
		}
	}

	private void makeConnection() {
		try {
			clientSocket = socket.accept();
			listening = true;
			System.out.println("Connected client: " + clientSocket.getInetAddress() + ":" + clientSocket.getPort());
		} catch (IOException e1) {
			System.err.println("Failed to open connection. Exiting...");
			return;
		}
	}

	private void processPacket(String line) {
		if (line.contains("[")) {
			String cmd = line.substring(0, line.indexOf("["));
			String args = line.substring(line.indexOf("[") + 1, line.indexOf("]"));
			line = line.replace("]", "");
			System.out.println("Received command: " + cmd);
			if (cmd.contentEquals("NewEntry")) {
				String bookName = line.substring(line.indexOf("=") + 1, line.indexOf(","));
				Contact c = data.addNewEntry(bookName, args);
				System.out.println("Saved entry to " + bookName + " as " + c.getName() + " " + c.getSurname() + " "
						+ c.getPhoneNumber());
				if (c != null) {
					send("NewEntry[result=succeeded,bn=" + bookName + ",name=" + c.getName() + ",surname="
							+ c.getSurname() + ",phone=" + c.getPhoneNumber() + "]");
				}
			} else if (cmd.contentEquals("NewAddressBook")) {
				data.createAddressBook(args);
				System.out.println("Created: " + args);
				send("NewAddressBook[result=succeeded,name=" + args + "]");
			} else if (cmd.contentEquals("ListBooks")) {
				String booksList = getBooksAsString();
				send(booksList);
			} else if(cmd.contentEquals("RequireContacts")) {
				send(getContactsAsString(args.split("=")[1]));
			}
		} else if (line.equals("Close")) {
			stop();
		} else {
			System.out.println("Ignored packet: " + line);
		}
	}

	private String getBooksAsString() {
		StringBuilder sb = new StringBuilder();
		List<String> books = data.getContainer().getAddressBooksNamesAsList();
		for (int i = 0; i < books.size(); i++) {
			sb.append("name").append(i).append("=").append(books.get(i));
			if (i < books.size() - 1)
				sb.append(",");
		}
		return "BookList[length=" + books.size() + "," + sb.toString() + "]";
	}

	public String getContactsAsString(String bookName) {
		StringBuilder sb = new StringBuilder();
		List<Contact> cs = data.getContainer().getContactsFromAddressBooks(bookName);
		sb.append("ContactList[length=").append(cs.size()).append(",");
		for (int i = 0; i < cs.size(); i++) {
			Contact c = cs.get(i);
			sb.append("index").append(i).append("=name:").append(c.getName()).append("|surname:").append(c.getSurname()).append("|phone:").append(c.getPhoneNumber());
			if(i<cs.size()-1)
				sb.append(",");
		}
		sb.append("]");
		return sb.toString();
	}

	private void send(String message) {
		try {
			out.write((message + "\n").getBytes());
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

	public synchronized void start() {
		daemonThread = new Thread(this, "AddressBookManager - Daemon");
		daemonThread.start();
	}

	public synchronized void stop() {
		listening = false;
		System.out.println("Closing connection to " + clientSocket.getInetAddress() + ":" + clientSocket.getPort());
		try {
			socket.close();
		} catch (IOException ioEx) {
			ioEx.printStackTrace();
		}
		try {
			clientSocket.close();
		} catch (IOException e) {
			e.printStackTrace();
		}
		daemonThread.interrupt();
		System.out.println("Writing files...");
		if (FileManager.save(data.getContainer()))
			System.out.println("Saved.");
		else
			System.err.println("Failed to save.");
	}

	public void setDataClass(Data d) {
		this.data = d;
	}

}
