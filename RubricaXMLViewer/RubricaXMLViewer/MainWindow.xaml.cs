using RubricaXMLViewer.AddressBook.Data.Network;
using RubricaXMLViewer.AddressBook.UI;
using RubricaXMLViewer.AddressBook.Utils;
using System.Windows;
using System.Windows.Controls;

namespace RubricaXMLViewer
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string selectedBook;

        public MainWindow()
        {
            NetworkManager.Instance.Initialize();
            InitializeComponent();
            UIProcessor.Instance.Init();
            Entries.ItemsSource = Instances.Entries;
            AddressBooks.ItemsSource = Instances.Books;
            AddressBooks.ContextMenu = AddressBooks.Resources["TreeViewRightClick"] as ContextMenu;
        }

        private void OnNewEntryAdd_Click(object sender, RoutedEventArgs e)
        {
            new EntryMaker(selectedBook).Show();
        }

        private void OnNewAddressBook_Click(object sender, RoutedEventArgs e)
        {
            new NewAddressBook().Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        { }

        private void AddressBooks_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            selectedBook = AddressBooks.SelectedItem.ToString();
        }
    }
}
