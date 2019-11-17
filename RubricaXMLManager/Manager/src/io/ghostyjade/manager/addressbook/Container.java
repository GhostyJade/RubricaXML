package io.ghostyjade.manager.addressbook;

import java.util.ArrayList;
import java.util.List;
import java.util.Map.Entry;
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
	
	public List<Contact> getContactsFromAddressBooks(String name){
		return books.get(name).getContacts();
	}

	@Override
	public Element toXML(Document d) {
		Element root = d.createElement("container");
		for (AddressBook addressBook : books.values()) {
			root.appendChild(addressBook.toXML(d));
		}
		return root;
	}
	
	public List<String> getAddressBooksNamesAsList() {
		List<String> names = new ArrayList<String>();
		for(Entry<String, AddressBook> pair : books.entrySet())
			names.add(pair.getKey());
		return names;
	}

}
