using RubricaXMLViewer.AddressBook.Data.Network;
using RubricaXMLViewer.AddressBook.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
            UIProcessor.Instance.Exit();
            NetworkManager.Instance.SendCloseMessage();
            NetworkManager.Instance.Close();
            base.OnExit(e);
        }
    }
}
