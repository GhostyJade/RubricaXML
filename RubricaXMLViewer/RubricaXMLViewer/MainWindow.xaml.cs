using RubricaXMLViewer.AddressBook.Data.Network;
using RubricaXMLViewer.AddressBook.Utils;
using System.Windows;

namespace RubricaXMLViewer
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            NetworkManager.Instance.Initialize();
            InitializeComponent();
            Entries.ItemsSource = Instances.Entries;
            AddressBooks.ItemsSource = Instances.Books;
        }

        private void OnNewEntryAdd_Click(object sender, RoutedEventArgs e)
        {
            new EntryMaker().Show();
        }

        private void OnNewAddressBook_Click(object sender, RoutedEventArgs e)
        {
            new NewAddressBook().Show();
        }
    }
}
