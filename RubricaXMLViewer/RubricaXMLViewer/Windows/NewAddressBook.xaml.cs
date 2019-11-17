using RubricaXMLViewer.AddressBook.Data.Network;
using RubricaXMLViewer.AddressBook.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RubricaXMLViewer.Windows
{
    /// <summary>
    /// Interaction logic for NewAddressBook.xaml
    /// </summary>
    public partial class NewAddressBook : Window
    {

        private readonly SolidColorBrush FF8A00FF = new SolidColorBrush(new Color() { R = 138, G = 0, B = 255, A = 255 });

        public NewAddressBook()
        {
            InitializeComponent();
        }

        private void CreateAddressBook_Click(object sender, RoutedEventArgs e)
        {
            if (txtAddressBookName.TryGetText(out string name))
            {
                NetworkManager.Instance.SendNewAddressBook(name);
                NetworkManager.Instance.Receive();
                Close();
            }
            else
            {
                lblNameError.Visibility = Visibility.Visible;
                txtAddressBookName.BorderBrush = Brushes.DarkRed;
            }
        }

        private void TxtAddressBookName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtAddressBookName.BorderBrush != FF8A00FF)
            {
                lblNameError.Visibility = Visibility.Hidden;
                txtAddressBookName.BorderBrush = FF8A00FF;
            }
        }
    }
}
