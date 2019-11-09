using RubricaXMLViewer.AddressBook.Data.Network;
using RubricaXMLViewer.AddressBook.Utils;
using System.Windows;

namespace RubricaXMLViewer
{
    /// <summary>
    /// Logica di interazione per EntryMaker.xaml
    /// </summary>
    public partial class EntryMaker : Window
    {
        public EntryMaker()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtName.TryGetText();
            string surname = TxtSurname.TryGetText();
            string phoneNumber = TxtPhoneText.TryGetText();
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phoneNumber))
            {
                //TODO perform something useful
                return;
            }else if(name.Contains(",")||surname.Contains(",")||phoneNumber.Contains(","))
            {
                //TODO perform something useful pt2
                return;
            }
            else
            {
                //FIXME AddressBook.Data.AddressBookEntry ent = new AddressBook.Data.AddressBookEntry(name, surname, phoneNumber, "", "", "", "", "", "");
                //TODO DataListener.Instance.SendNewAddressBookEntry(ent);
            }
        }
    }
}
