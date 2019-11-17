using RubricaXMLViewer.AddressBook.Data.Network;
using RubricaXMLViewer.AddressBook.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RubricaXMLViewer.Windows
{
    /// <summary>
    /// Logica di interazione per ChangeAddressBook.xaml
    /// </summary>
    public partial class ChangeAddressBook : Window
    {

        private readonly ObservableCollection<string> list;

        public ChangeAddressBook(List<string> bookNames)
        {
            list= new ObservableCollection<string>(bookNames);
            InitializeComponent();
            CmbBookName.ItemsSource = list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CmbBookName.SelectedItem != null)
            {
                string name = CmbBookName.SelectedItem as string;
                NetworkManager.Instance.RequireContactList(name);
                NetworkManager.Instance.Receive();
                Close();
            }
            else
            {
                //show label
            }
        }
    }
}
