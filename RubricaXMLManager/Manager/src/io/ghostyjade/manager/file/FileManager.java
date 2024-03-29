package io.ghostyjade.manager.file;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.OutputKeys;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerConfigurationException;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;

import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;

import io.ghostyjade.manager.addressbook.AddressBook;
import io.ghostyjade.manager.addressbook.Contact;
import io.ghostyjade.manager.addressbook.Container;

public class FileManager {

	private static final String FILE_NAME = "rubrica.xml";

	public static Container read() {
		Container c = new Container();
		if(checkForFileExistence()) {
			Document d = getDocumentFromFile(FILE_NAME);
			parseFile(d, c);
		}
		return c;
	}
	
	private static void parseFile(Document d, Container c) {
		Element root = d.getDocumentElement();
		NodeList addressBooks = root.getElementsByTagName("addressbook");
		c.setLastId(addressBooks.getLength());
		for (int i = 0; i < addressBooks.getLength(); i++) {
			Node book = addressBooks.item(i);
			String bookName = book.getAttributes().getNamedItem("name").getNodeValue();
			c.addAddressBook(new AddressBook(i, bookName));
			NodeList entries = book.getChildNodes();
			for(int j = 0; j < entries.getLength(); j++) {
				Node entry = entries.item(j);
				if(entry.getNodeType() != Node.TEXT_NODE) {
					int id = Integer.valueOf(entry.getAttributes().getNamedItem("id").getNodeValue());
					NodeList subNodes = entry.getChildNodes();
					String name = null, surname = null, phone = null;
					for(int k = 0; k < subNodes.getLength(); k++) {
						Node attribute = subNodes.item(k);
						if(attribute.getNodeType()!=Node.TEXT_NODE) {
							String value = attribute.getTextContent();
							switch(attribute.getNodeName()) {
							case "name":
								name=value;
								break;
							case "surname":
								surname=value;
								break;
							case "phone":
								phone=value;
								break;
							}
						}
					}
					c.addExistingContact(bookName, new Contact(id, name, surname, phone));
				}
			}
		}
	}
	
	@Deprecated
	public static List<Contact> readAddressBook(String filename) {
		List<Contact> entries = new ArrayList<Contact>();
		Document d = getDocumentFromFile(filename);
		d.getDocumentElement().normalize();
		Element root = d.getDocumentElement();
		NodeList elements = root.getElementsByTagName("entry");
		for (int i = 0; i < elements.getLength(); i++) {
			Node n = elements.item(i);
			if (n.getNodeType() == Node.ELEMENT_NODE)
				entries.add(getEntryFromNode(n));
		}
		return entries;
	}

	@Deprecated
	private static Contact getEntryFromNode(Node n) {
		Element data = (Element) n;
		int id = Integer.valueOf(data.getAttribute("id"));
		String name = data.getElementsByTagName("name").item(0).getTextContent();
		String surname = data.getElementsByTagName("surname").item(0).getTextContent();
		String phoneNumber = data.getElementsByTagName("phone").item(0).getTextContent();
		Contact e = new Contact(id, name, surname, phoneNumber);
		return e; // TODO implement all infos to all entries
	}

	private static Document getDocumentFromFile(String filename) {
		DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
		try {
			DocumentBuilder builder = dbf.newDocumentBuilder();
			Document d = builder.parse(new File(filename));
			d.normalize();
			return d;
		} catch (ParserConfigurationException e) {
			e.printStackTrace();
		} catch (SAXException e) {
			System.err.println();
			e.printStackTrace();
		} catch (IOException e) {
			System.err.println("File not found: " + filename);
			e.printStackTrace();
		}
		return null;
	}

	public static boolean checkForFileExistence() {
		File addressBook = new File(FILE_NAME);
		return addressBook.exists();
	}

	public static boolean save(Container c) {
		DocumentBuilderFactory documentFactory = DocumentBuilderFactory.newInstance();
		DocumentBuilder documentBuilder = null;
		try {
			documentBuilder = documentFactory.newDocumentBuilder();
		} catch (ParserConfigurationException e2) {
			e2.printStackTrace();
			return false;
		}
		Document d = documentBuilder.newDocument();
		Node root = c.toXML(d);
		d.appendChild(root);
		TransformerFactory f = TransformerFactory.newInstance();
		Transformer t = null;
		try {
			t = f.newTransformer();
			t.setOutputProperty(OutputKeys.INDENT, "yes");
		} catch (TransformerConfigurationException e) {
			e.printStackTrace();
			return false;
		}
		DOMSource src = new DOMSource(d);
		File file = new File(FILE_NAME);
		if (!file.exists())
			try {
				file.createNewFile();
			} catch (IOException e1) {
				e1.printStackTrace();
				return false;
			}
		StreamResult data = new StreamResult(file);
		try {
			t.transform(src, data);
			return true;
		} catch (TransformerException e) {
			e.printStackTrace();
			return false;
		}
	}

}
