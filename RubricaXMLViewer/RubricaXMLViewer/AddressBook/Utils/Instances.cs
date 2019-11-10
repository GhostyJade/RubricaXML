using RubricaXMLViewer.AddressBook.Data;
using System.Collections.ObjectModel;

namespace RubricaXMLViewer.AddressBook.Utils
{
    public static class Instances
    {
        public static ObservableCollection<AddressBookEntry> Entries = new ObservableCollection<AddressBookEntry>();
        public static ObservableCollection<string> Books = new ObservableCollection<string>();
    }
}
