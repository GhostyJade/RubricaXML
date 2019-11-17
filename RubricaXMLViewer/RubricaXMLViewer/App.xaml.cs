using RubricaXMLViewer.AddressBook.Data.Network;
using System.Windows;

namespace RubricaXMLViewer
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnExit(ExitEventArgs e)
        {
            NetworkManager.Instance.SendCloseMessage();
            NetworkManager.Instance.Close();
            base.OnExit(e);
        }
    }
}
