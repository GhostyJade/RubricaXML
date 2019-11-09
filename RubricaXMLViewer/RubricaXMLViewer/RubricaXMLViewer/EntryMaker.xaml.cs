using RubricaXMLViewer.AddressBook.Data.Network;
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
            MessageBox.Show("WA");
            if (string.IsNullOrEmpty(TxtName.Text) || string.IsNullOrEmpty(TxtPhoneText.Text))
            {
                //TODO perform something useful
                return;
            }
            else
            {
                AddressBook.Data.AddressBookEntry ent = new AddressBook.Data.AddressBookEntry(TxtName.Text, TxtSurname.Text, TxtPhoneText.Text, "", "", "", "", "", "");
                DataListener.Instance.SendNewAddressBookEntry(ent);
            }
        }
    }
}
