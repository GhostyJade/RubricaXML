using System.Net;
using System.Net.Sockets;
using System;
using System.Windows;
using System.Text;
using System.Threading;

namespace RubricaXMLViewer.AddressBook.Data.Network
{
    public class DataListener
    {

        public static DataListener Instance { get; } = new DataListener();

        private DataListener()
        {
        }

        public void Initialize()
        {
            AsyncClient.StartClient();
        }

        public void Receive()
        {

        }

        public void SendCloseMessage()
        {
            Send("Close");
        }

        public void Send(string data)
        {
            AsyncClient.Send(data + "\n");
        }

        public void SendNewAddressBookEntry(AddressBookEntry e)
        {
            string data = string.Format("CmdSave[Name={0},Surname={1},PhoneNumber={2}]", e.Name, e.Surname, e.PhoneNumber);
            Send(data);
        }

    }
}
