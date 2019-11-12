package io.ghostyjade.manager.data;

import io.ghostyjade.manager.addressbook.AddressBook;
import io.ghostyjade.manager.addressbook.Container;

public class Data {

	private Container container;
	
	public Data() {
		this.container = new Container();
	}

	public void setFromExistingContainer(Container c) {
		this.container = c;
	}

	public Container getContainer() {
		return container;
	}

	public void createAddressBook(String name) {
		container.addAddressBook(new AddressBook(0, name));
	}

	public void addNewEntry(String bookName, String data) {
		container.addNewEntry(bookName, data);
	}

}
