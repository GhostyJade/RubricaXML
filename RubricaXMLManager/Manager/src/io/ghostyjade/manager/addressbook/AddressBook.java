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
	
	private int lastId=0;

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
		root.setAttribute("name", name);
		for (Contact contact : contacts) {
			root.appendChild(contact.toXML(d));
		}
		return root;
	}
	
	public void addEntry(String data) {
		addEntry(data.split(","));
	}
	
	private void addEntry(String[] parts) {
		Map<String, String> data = new HashMap<String, String>();
		for (String s : parts) {
			String[] p = s.split("=");
			data.put(p[0], p[1]);
		}
		contacts.add(new Contact(lastId++, data.get("name"), data.get("surname"), data.get("phone")));
	}

	public void deleteEntry(String phone) {
		Contact toRemoveEntry = new Contact();
		for (Contact e : contacts) {
			if(e.getPhoneNumber().equals(phone)) {
				toRemoveEntry = e;
				return;
			}
		}
		
		if(toRemoveEntry != null)
			contacts.remove(toRemoveEntry);
	}

}
