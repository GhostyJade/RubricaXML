using System.Collections.Generic;
using RubricaXMLViewer.AddressBook.Utils;

namespace RubricaXMLViewer.AddressBook.Data
{
    public class AddressBookEntry
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }

        public string EntryMail { get; set; }
        public string Address { get; set; }
        public string WebAddress { get; set; }
        public string Notes { get; set; }
        public string BornDate { get; set; }
        public string Nickname { get; set; }

        /*public AddressBookEntry(string name, string surname, string pn, string em, string address, string wa, string notes, string bd, string nickname)
        {
            Name = name;
            Surname = surname;
            PhoneNumber = pn;
            EntryMail = em;
            Address = address;
            WebAddress = wa;
            Notes = notes;
            BornDate = bd;
            Nickname = nickname;
        }*/
        public AddressBookEntry() { }

        public AddressBookEntry(string data)
        {
            Dictionary<string, string> entryData = new Dictionary<string, string>();
            foreach(string s in data.Split(','))
            {
                string[] p = s.Split('=');
                entryData.Add(p[0], p[1]);
            }
            Name = entryData.GetData("name");
            Surname = entryData.GetData("surname");
            PhoneNumber = entryData.GetData("phone");
        }

       
    }
}
