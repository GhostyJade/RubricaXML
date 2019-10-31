package io.ghostyjade.manager.addressbook;

public class AddressBookEntry {

	//Required infos
	private String name;
	private String surname;
	private String phoneNumber;

	private String entryMail;
	private String address;
	private String webAddress;
	private String notes;
	private String bornDate;
	private String nickname;

	public AddressBookEntry(String name, String surname, String phoneNumber) {
		this.name = name;
		this.surname = surname;
		this.phoneNumber=phoneNumber;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public String getSurname() {
		return surname;
	}

	public void setSurname(String surname) {
		this.surname = surname;
	}

	public String getEntryMail() {
		return entryMail;
	}

	public void setEntryMail(String entryMail) {
		this.entryMail = entryMail;
	}

	public String getPhoneNumber() {
		return phoneNumber;
	}

	public void setPhoneNumber(String phoneNumber) {
		this.phoneNumber = phoneNumber;
	}

	public String getAddress() {
		return address;
	}

	public void setAddress(String address) {
		this.address = address;
	}

	public String getWebAddress() {
		return webAddress;
	}

	public void setWebAddress(String webAddress) {
		this.webAddress = webAddress;
	}

	public String getNotes() {
		return notes;
	}

	public void setNotes(String notes) {
		this.notes = notes;
	}

	public String getBornDate() {
		return bornDate;
	}

	public void setBornDate(String bornDate) {
		this.bornDate = bornDate;
	}

	public String getNickname() {
		return nickname;
	}

	public void setNickname(String nickname) {
		this.nickname = nickname;
	}

}
