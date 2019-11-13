package io.ghostyjade.manager.addressbook;

import java.util.concurrent.ConcurrentHashMap;

import org.w3c.dom.Document;
import org.w3c.dom.Element;

public class Container implements XMLSerializable {

	private ConcurrentHashMap<String, AddressBook> books = new ConcurrentHashMap<>();
	private int lastId;

	public Container() {
	}

	public void setLastId(int id) {
		this.lastId = id;
	}

	public void addAddressBook(AddressBook a) {
		books.put(a.getName(), a);
	}

	public Contact addNewEntry(String bookName, String data) {
		return books.get(bookName).addEntry(data);
	}
	
	public void addExistingContact(String bookname, Contact data) {
		books.get(bookname).addEntry(data);
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
