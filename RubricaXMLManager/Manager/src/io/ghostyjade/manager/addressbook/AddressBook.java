package io.ghostyjade.manager.addressbook;

import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.CopyOnWriteArrayList;

import org.w3c.dom.Document;
import org.w3c.dom.Element;

public class AddressBook implements XMLSerializable {

	private int id;
	private String name;

	private List<Contact> contacts = new CopyOnWriteArrayList<Contact>();

	private static int lastId = 0;

	public static void setLastId(int value) {
		lastId = value;
	}

	public AddressBook(int id, String name) {
		this.id = id;
		this.name = name;
	}

	public String getName() {
		return name;
	}

	@Override
	public Element toXML(Document d) {
		Element root = d.createElement("addressbook");
		root.setAttribute("id", String.valueOf(id));
		//root.setAttribute("lastid", String.valueOf(lastId));
		root.setAttribute("name", name);
		for (Contact contact : contacts) {
			root.appendChild(contact.toXML(d));
		}
		return root;
	}

	public Contact addEntry(String data) {
		return addEntry(data.split(","));
	}
	
	public void addEntry(Contact c) {
		contacts.add(c);
	}

	private Contact addEntry(String[] parts) {
		Map<String, String> data = new HashMap<String, String>();
		for (String s : parts) {
			String[] p = s.split("=");
			data.put(p[0], p[1]);
		}
		Contact c = new Contact(lastId++, data.get("name"), data.get("surname"), data.get("phone"));
		contacts.add(c);
		return c;
	}

	public void deleteEntry(String phone) {
		Contact toRemoveEntry = new Contact();
		for (Contact e : contacts) {
			if (e.getPhoneNumber().equals(phone)) {
				toRemoveEntry = e;
				return;
			}
		}

		if (toRemoveEntry != null)
			contacts.remove(toRemoveEntry);
	}
	
	@Override
	public String toString() {
		return "Name: " + name;
	}

	public List<Contact> getContacts() {
		return contacts;
	}

}
