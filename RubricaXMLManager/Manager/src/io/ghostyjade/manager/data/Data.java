package io.ghostyjade.manager.data;

import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.CopyOnWriteArrayList;

import io.ghostyjade.manager.addressbook.Contact;

public class Data {

	private List<Contact> entries = new CopyOnWriteArrayList<>();

	private int lastId = 0;

	public void addEntry(String data) {
		addEntry(data.split(","));
	}

	public void addEntry(String[] parts) {
		Map<String, String> data = new HashMap<String, String>();
		for (String s : parts) {
			String[] p = s.split("=");
			data.put(p[0], p[1]);
		}
		entries.add(new Contact(lastId++, data.get("name"), data.get("surname"), data.get("phone")));
	}

	public void deleteEntry(String phone) {
		Contact toRemoveEntry = null;
		for (Contact e : entries) {
			if(e.getPhoneNumber().equals(phone)) {
				toRemoveEntry = e;
				return;
			}
		}
		if(toRemoveEntry != null)
			entries.remove(toRemoveEntry);
	}

	public List<Contact> getEntries() {
		return entries;
	}

}
