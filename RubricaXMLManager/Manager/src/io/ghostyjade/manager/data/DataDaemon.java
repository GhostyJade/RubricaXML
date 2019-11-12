package io.ghostyjade.manager.data;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.ServerSocket;
import java.net.Socket;

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
			String cmd=line.substring(0, line.indexOf("["));
			String args = line.substring(line.indexOf("[")+1, line.indexOf("]"));
			line = line.replace("]", "");
			System.out.println("Received command: " + cmd);
			if (cmd.contentEquals("NewEntry")) {
				String bookName = line.substring(0, line.indexOf(","));
				args=args.replace(bookName, "");
				data.addNewEntry(bookName, args);
				System.out.println("Saved entry");
			}else if(cmd.contentEquals("NewAddressBook")) {
				data.createAddressBook(args);
				System.out.println("Created: " + args);
				send("NewAddressBook[result=succeeded,name=" + args + "]");
			}
		} else if (line.equals("Close")) {
			stop();
		} else {
			System.out.println("Ignored packet: " + line);
		}
	}

	private void send(String message) {
		try {
			out.write(message.getBytes());
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
