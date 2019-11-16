using RubricaXMLViewer.AddressBook.Data;
using System.Collections.ObjectModel;

namespace RubricaXMLViewer.AddressBook.Utils
{
    public static class Instances
    {
        public static volatile ObservableCollection<AddressBookEntry> Entries = new ObservableCollection<AddressBookEntry>();
        public static volatile ObservableCollection<string> Books = new ObservableCollection<string>();
    }
}
