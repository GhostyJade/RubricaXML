package io.ghostyjade.manager.addressbook;

import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;

public class Contact implements XMLSerializable{

	// Required infos
	private int id;

	private String name;
	private String surname;
	private String phoneNumber;

	private String entryMail;
	private String address;
	private String webAddress;
	private String notes;
	private String bornDate;
	private String nickname;

	public Contact(int id, String name, String surname, String phoneNumber) {
		this.name = name;
		this.surname = surname;
		this.phoneNumber = phoneNumber;
	}
	
	public Contact() {
	}
	
	public String getName() {
		return name;
	}
	
	public String getSurname() {
		return surname;
	}

	public String getPhoneNumber() {
		return phoneNumber;
	}

	@Override
	public String toString() {
		return "Name: " + name + ", Surname: " + surname + ", Phone number: " + phoneNumber;
	}
	
	@Override
	public Element toXML(Document d) {
		Element element = d.createElement("entry");
		element.setAttribute("id", String.valueOf(id));
		element.appendChild(createNode(d, "name", name));
		element.appendChild(createNode(d, "surname", surname));
		element.appendChild(createNode(d, "phone", phoneNumber));
		return element;
	}

	public String toUIString() {
		StringBuilder sb = new StringBuilder("[Contact]");
		sb.append("name=").append(name);
		sb.append("surname=").append(surname);
		sb.append("phone=").append(phoneNumber);
		return sb.toString();
	}

	private Node createNode(Document d, String name, String value) {
		Node n = d.createElement(name);
		n.appendChild(d.createTextNode(value));
		return n;
	}

}
