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
	private boolean listening = false;
	private ServerSocket socket;
	private Data data;
	private Socket clientSocket;

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
		BufferedReader in = null;
		try {
			in = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));
			DataOutputStream out = new DataOutputStream(clientSocket.getOutputStream());
		} catch (IOException e1) {
			e1.printStackTrace();
		}

		while (listening) {
			try {
				
				processPacket(in.readLine());
			} catch (IOException e) {
				// e.printStackTrace();
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
			String cmd = line.substring(line.indexOf("["), line.indexOf("]"));
			System.out.println("Received command: " + cmd);
			if (cmd.equals("CmdSave")) {
				line = line.replace("]", "");
				String[] parts = line.replace("CmdSave ", "").split(",");
				data.addEntry(parts);
				System.out.println("Saved entry");
			}
		} else if (line.equals("Close")) {
			stop();
		} else {
			System.out.println("Ignored packet: " + line);
		}
	}

	private void send() {

	}

	public synchronized void start() {
		daemonThread = new Thread(this, "AddressBookManager - Daemon");
		daemonThread.start();
	}

	public synchronized void stop() {
		System.out.println("Closing connection to " + clientSocket.getInetAddress() + ":" + clientSocket.getPort());
		try {
			clientSocket.close();
		} catch (IOException e) {
			e.printStackTrace();
		}
		try {
			socket.close();
		} catch (IOException ioEx) {
			ioEx.printStackTrace();
		}
		listening=false;
		try {
			daemonThread.join();
		} catch (InterruptedException iEx) {
			iEx.printStackTrace();
		}
		System.out.println("Writing files...");
		FileManager.saveAddressBook(filename, entries);
	}

	public void setDataClass(Data d) {
		this.data = d;
	}

}
