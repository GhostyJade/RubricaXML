package io.ghostyjade.manager.data;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.ServerSocket;
import java.net.Socket;

public class DataDaemon implements Runnable {
	
	private Thread daemonThread;
	private boolean listening = false;
	private ServerSocket socket;
	private Data data;

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
		while (listening) {
			try {
				Socket clientSocket = socket.accept();
				BufferedReader in = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));
				DataOutputStream out = new DataOutputStream(clientSocket.getOutputStream());
				processPacket(in.readLine());					
			} catch (IOException e) {
				e.printStackTrace();
			}
		}
	}
	
	private void processPacket(String line) {
		String cmd = line.substring(0,8);
		System.out.println(cmd);
		if(cmd.equals("CmdSave ")) {
			String[] parts = line.replace("CmdSave ", "").split(",");
			data.addEntry(parts);
			System.out.println("Saved entry");
		}
	}
	
	private void send() {
		
	}

	public synchronized void start() {
		listening = true;
		daemonThread = new Thread(this, "AddressBookManager - Daemon");
		daemonThread.start();
	}

	public synchronized void stop() {
		try {
			daemonThread.join();
		} catch (InterruptedException e) {
			e.printStackTrace();
		}
		try {
			socket.close();
		} catch (IOException e1) {
			e1.printStackTrace();
		}
	}

	public void setDataClass(Data d) {
		this.data = d;
	}

}
