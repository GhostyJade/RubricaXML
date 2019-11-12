using RubricaXMLViewer.AddressBook.Data.Network;
using RubricaXMLViewer.AddressBook.UI;
using RubricaXMLViewer.AddressBook.Utils;
using System.Windows;
using System.Threading;

namespace RubricaXMLViewer
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool running = false;

        private Thread uiThread;

        public MainWindow()
        {
            NetworkManager.Instance.Initialize();
            InitializeComponent();
            UIProcessor.Instance.Init(this);
            Entries.ItemsSource = Instances.Entries;
            AddressBooks.ItemsSource = Instances.Books;
            uiThread = new Thread(new ThreadStart(() => { while (running) { UIProcessor.Instance.Update(); } }));
            running = true;
            uiThread.Start();
        }

        private void OnNewEntryAdd_Click(object sender, RoutedEventArgs e)
        {
            new EntryMaker().Show();
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
    }
}
