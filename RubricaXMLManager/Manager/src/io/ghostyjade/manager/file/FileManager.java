package io.ghostyjade.manager.file;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;

import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;

import io.ghostyjade.manager.addressbook.AddressBookEntry;

public class FileManager {
	
	private static final String FILE_PATH = "";
	private static final String FILE_NAME = "rubrica.xml";
	
	public static void saveAddressBook(String filename) {
		
	}
	
	public static List<AddressBookEntry> readAddressBook(String filename) {
		List<AddressBookEntry> entries = new ArrayList<AddressBookEntry>();
		Document d = getDocumentFromFile(filename);
		d.getDocumentElement().normalize();
		Element root = d.getDocumentElement();
		NodeList elements = root.getElementsByTagName("contacts");
		for(int i = 0; i < elements.getLength(); i++) {
			Node n = elements.item(i);
			
		}
		return entries;
	}
	
	private static AddressBookEntry getEntryFromNode(Node n) {
		
		return null;
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
