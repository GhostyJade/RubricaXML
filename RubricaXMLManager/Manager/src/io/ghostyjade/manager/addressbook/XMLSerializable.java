package io.ghostyjade.manager.addressbook;

import org.w3c.dom.Document;
import org.w3c.dom.Element;

public interface XMLSerializable {

	public Element toXML(Document d);
	
}
