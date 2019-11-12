using RubricaXMLViewer.AddressBook.UI;
using RubricaXMLViewer.AddressBook.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace RubricaXMLViewer.AddressBook.Data.Network
{
    public class NetworkManager
    {

        public static NetworkManager Instance { get; } = new NetworkManager();

        private NetworkManager()
        {
        }

        public void Initialize()
        {
            AsyncClient.StartClient();
        }

        public void Receive()
        {
            RunThread();
        }

        private void RunThread()
        {
            new Thread(new ThreadStart(() =>
            {
                AsyncClient.Receive();
                AsyncClient.receiveDone.WaitOne();
                string msg = AsyncClient.GetResponse();
                if (msg != null)
                {
                    ProcessMessage(msg, out string action, out string[] args);
                    Console.WriteLine(msg);
                    UIProcessor.Instance.ParseAction(action, args);
                }
            })).Start();
        }

        private void ProcessMessage(string message, out string action, out string[] args)
        {
            action = message.Substring(0, message.IndexOf("["));
            args = message.Substring(message.IndexOf("[") + 1, message.LastIndexOf("]") - message.IndexOf("[")-1).Split(',');
        }

        public void SendCloseMessage()
        {
            Send("Close");
        }

        public void Close()
        {
            AsyncClient.Close();
        }

        public void Send(string data)
        {
            AsyncClient.Send(data + "\n");
        }

        public void SendNewAddressBookEntry(AddressBookEntry e, string addressBookName)
        {
            string data = string.Format("NewEntry[bn={0},name={1},surname={2},phone={3}]", addressBookName, e.Name, e.Surname, e.PhoneNumber);
            Send(data);
        }

        public void SendNewAddressBook(string name)
        {
            string data = string.Format("NewAddressBook[{0}]", name);
            Send(data);
        }

    }
}
