using RubricaXMLViewer.AddressBook.Data;
using RubricaXMLViewer.AddressBook.Data.Network;
using System.Collections.ObjectModel;
using System.Windows;

namespace RubricaXMLViewer
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ObservableCollection<AddressBookEntry> entries = new ObservableCollection<AddressBookEntry>();

        public MainWindow()
        {
            InitializeComponent();
            new EntryMaker().Show();
            Entries.ItemsSource = entries;
            DataListener.Instance.Connect();
        }
    }
}
