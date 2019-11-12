using RubricaXMLViewer.AddressBook.Data.Network;
using RubricaXMLViewer.AddressBook.UI;
using RubricaXMLViewer.AddressBook.Utils;
using System.Windows;
using System.Windows.Media;

namespace RubricaXMLViewer
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private UIProcessor uiProcessor;

        protected override void OnRender(DrawingContext drawingContext)
        {
            uiProcessor.Update();
            base.OnRender(drawingContext);
        }

        public MainWindow()
        {
            NetworkManager.Instance.Initialize();
            InitializeComponent();
            uiProcessor = new UIProcessor(this);
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
