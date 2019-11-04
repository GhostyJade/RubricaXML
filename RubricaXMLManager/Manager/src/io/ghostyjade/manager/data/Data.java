package io.ghostyjade.manager.data;

import java.util.List;
import java.util.concurrent.CopyOnWriteArrayList;

import io.ghostyjade.manager.addressbook.AddressBookEntry;

public class Data {

	private List<AddressBookEntry> entries = new CopyOnWriteArrayList<>();
	
	public void addEntry(String[] parts) {
		String name = parts[0].substring(5);
		String surname = parts[1].substring(8);
		String phoneNumber = parts[2].substring(12);
		entries.add(new AddressBookEntry(name, surname, phoneNumber));
		System.out.println(entries.get(0));
	}
	
	

}
