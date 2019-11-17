using RubricaXMLViewer.AddressBook.Data;
using RubricaXMLViewer.AddressBook.Data.Network;
using RubricaXMLViewer.AddressBook.UI;
using RubricaXMLViewer.AddressBook.Utils;
using RubricaXMLViewer.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace RubricaXMLViewer
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private string selectedBook;

        public static MainWindow Instance { get; private set; }
        private ObservableCollection<AddressBookEntry> EntriesData = new ObservableCollection<AddressBookEntry>();

        public MainWindow()
        {
            NetworkManager.Instance.Initialize();
            InitializeComponent();
            Instance = this;
            UIProcessor.Instance.Init();
            Entries.ItemsSource = EntriesData;
            //AddressBooks.ContextMenu = AddressBooks.Resources["TreeViewRightClick"] as ContextMenu;
        }

        private void OnNewEntryAdd_Click(object sender, RoutedEventArgs e)
        {
            //new EntryMaker(selectedBook).Show();
        }

        private void OnNewAddressBook_Click(object sender, RoutedEventArgs e)
        {
            new NewAddressBook().Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void AddressBooks_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //selectedBook = AddressBooks.SelectedItem.ToString();
        }

        private void Entries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            NetworkManager.Instance.AskForAddressBooks();
            NetworkManager.Instance.Receive();
        }

        public void SetData(List<AddressBookEntry> entries)
        {
            //Dispatcher.Invoke(() => { 
                EntriesData.Clear();
                EntriesData.AddRange(entries);
            //});
        }

        public void AddEntry(AddressBookEntry e)
        {
           // Dispatcher.Invoke(() => {
                EntriesData.Add(e);
           // });
        }
    }
}
