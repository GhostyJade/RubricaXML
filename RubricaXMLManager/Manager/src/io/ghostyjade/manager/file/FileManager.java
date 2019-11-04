package io.ghostyjade.manager.file;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerConfigurationException;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;

import org.w3c.dom.Attr;
import org.w3c.dom.DOMException;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.NamedNodeMap;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.w3c.dom.TypeInfo;
import org.w3c.dom.UserDataHandler;
import org.xml.sax.SAXException;

import io.ghostyjade.manager.addressbook.AddressBookEntry;

public class FileManager {

	private static final String FILE_PATH = "";
	private static final String FILE_NAME = "rubrica.xml";

	public static void saveAddressBook(String filename, List<AddressBookEntry> entries) {
		DocumentBuilderFactory documentFactory = DocumentBuilderFactory.newInstance();
        DocumentBuilder documentBuilder = null;
		try {
			documentBuilder = documentFactory.newDocumentBuilder();
		} catch (ParserConfigurationException e2) {
			e2.printStackTrace();
		}
        Document d = documentBuilder.newDocument();
		Node root = d.createElement("addressbook");
		d.appendChild(root);
        for (AddressBookEntry addressBookEntry : entries) {
        	root.appendChild(getNodeFromEntry(addressBookEntry, d));
		}
		TransformerFactory f = TransformerFactory.newInstance();
		Transformer t = null;
		try {
			t = f.newTransformer();
		} catch (TransformerConfigurationException e) {
			e.printStackTrace();
		}
		DOMSource src = new DOMSource(d);
		File file = new File(filename);
		if(!file.exists())
			try {
				file.createNewFile();
			} catch (IOException e1) {
				e1.printStackTrace();
			}
		StreamResult data = new StreamResult(file);
		try {
			t.transform(src, data);
		} catch (TransformerException e) {
			e.printStackTrace();
		}
	}

	public static List<AddressBookEntry> readAddressBook(String filename) {
		List<AddressBookEntry> entries = new ArrayList<AddressBookEntry>();
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

	private static AddressBookEntry getEntryFromNode(Node n) {
		Element data = (Element) n;
		String name = data.getElementsByTagName("name").item(0).getTextContent();
		String surname = data.getElementsByTagName("surname").item(0).getTextContent();
		String phoneNumber = data.getElementsByTagName("phoneNumber").item(0).getTextContent();
		AddressBookEntry e = new AddressBookEntry(name, surname, phoneNumber);
		return e; //TODO implement all infos and add id to all entries
	}
	
	private static Element getNodeFromEntry(AddressBookEntry e, Document d) {
		Element element = d.createElement("entry");
		element.setAttribute("id", "0");
		//element.setAttribute("name", e.getName());
		//element.setAttribute("surname", e.getSurname());
		//element.setAttribute("phoneNumber", e.getPhoneNumber());
		element.appendChild(createNode(d, "name", e.getName()));
		element.appendChild(createNode(d, "surname", e.getSurname()));
		element.appendChild(createNode(d, "phoneNumber", e.getPhoneNumber()));
		return element;
	}
	
	private static Node createNode(Document d, String name, String value) {
		Node n = d.createElement(name);
		n.appendChild(d.createTextNode(value));
		return n;
	}

	private static Document getDocumentFromFile(String filename) {
		DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
		try {
			DocumentBuilder builder = dbf.newDocumentBuilder();
			Document d = builder.parse(new File(filename));
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

}
