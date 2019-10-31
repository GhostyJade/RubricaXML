namespace RubricaXMLViewer.AddressBook.Data
{
    public class AddressBookEntry
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string PhoneNumber { get; private set; }

        public string EntryMail { get; private set; }
        public string Address { get; private set; }
        public string WebAddress { get; private set; }
        public string Notes { get; private set; }
        public string BornDate { get; private set; }
        public string Nickname { get; private set; }

        public AddressBookEntry(string name, string surname, string pn, string em, string address, string wa, string notes, string bd, string nickname)
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
        }
    }
}
