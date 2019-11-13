using RubricaXMLViewer.AddressBook.Data.Network;
using RubricaXMLViewer.AddressBook.UI;
using RubricaXMLViewer.AddressBook.Utils;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace RubricaXMLViewer
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool running = false;

        private string selectedBook;

        private Thread uiThread;

        public MainWindow()
        {
            NetworkManager.Instance.Initialize();
            InitializeComponent();
            UIProcessor.Instance.Init(this);
            Entries.ItemsSource = Instances.Entries;
            AddressBooks.ItemsSource = Instances.Books;
            AddressBooks.ContextMenu = AddressBooks.Resources["TreeViewRightClick"] as ContextMenu;
            running = true;

            uiThread = new Thread(() =>
            {
                while (running)
                {
                    UIProcessor.Instance.Update();
                }
            });
            uiThread.Start();
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
        {
            running = false;
            uiThread.Join();
        }

        private void AddressBooks_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            selectedBook = AddressBooks.SelectedItem.ToString();
        }
    }
}
