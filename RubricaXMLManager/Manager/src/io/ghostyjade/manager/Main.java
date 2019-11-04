package io.ghostyjade.manager;

import io.ghostyjade.manager.data.Data;
import io.ghostyjade.manager.data.DataDaemon;

public class Main {
	
	public static void main(String[] args) {
		if(args[0].equals("nogui")) {
			String address = args[1];
			int port = Integer.valueOf(args[2]);
			DataDaemon daemon = new DataDaemon(address, port);
			Data d = new Data();
			daemon.setDataClass(d);
			daemon.start();		
		}else {
			System.out.println("This program is useless without it's GUI...Maybe...\nIf you need to start only the server, use: server.jar nogui [address] [port]" );
		}
	}
}
