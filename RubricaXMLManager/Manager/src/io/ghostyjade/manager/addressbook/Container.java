package io.ghostyjade.manager.addressbook;

import java.util.concurrent.ConcurrentHashMap;

import org.w3c.dom.Document;
import org.w3c.dom.Element;

public class Container implements XMLSerializable {
	
	private ConcurrentHashMap<String, AddressBook> books = new ConcurrentHashMap<>();

	public Container() {
	}
	
	public void addAddressBook(AddressBook a) {
		books.put(a.getName(), a);
	}
	
	/*public void addNewEntry(String bookName, Contact c) {
		books.get(bookName).addEntry(c);
	}*/
	
	public void addNewEntry(String bookName, String data) {
		books.get(bookName).addEntry(data);
	}
	
	@Override
	public Element toXML(Document d) {
		Element root = d.createElement("container");
		for (AddressBook addressBook : books.values()) {
			root.appendChild(addressBook.toXML(d));
		}
		return root;
	}
	
	

}
