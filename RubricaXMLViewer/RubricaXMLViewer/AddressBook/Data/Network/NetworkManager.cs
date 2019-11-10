using System.Net;
using System.Net.Sockets;
using System;
using System.Windows;
using System.Text;
using System.Threading;
using RubricaXMLViewer.AddressBook.Utils;
using System.Collections.Generic;

namespace RubricaXMLViewer.AddressBook.Data.Network
{
    public class NetworkManager
    {
        private Thread queueThread;
        private Thread receiveThread;

        public static NetworkManager Instance { get; } = new NetworkManager();

        private NetworkManager()
        {
        }

        public void Initialize()
        {
            AsyncClient.StartClient();
            receiveThread = new Thread(new ThreadStart(Receive));
            queueThread = new Thread(new ThreadStart(DispatchQueue));
            //queueThread.Start();
        }

        public void Receive()
        {
            new Thread(new ThreadStart(WaitForMessages)).Start();

        }

        private void WaitForMessages()
        {
            AsyncClient.Receive();
            //AsyncClient.receiveDone.WaitOne();
            foreach(string s in AsyncClient.messageQueue.ToArray()){
                Console.WriteLine(s);
            }
        }

        public void SendCloseMessage()
        {
            Send("Close");
        }

        public void Close()
        {
            //queueThread.Join();
            //receiveThread.Join();
            AsyncClient.Close();
        }

        public void Send(string data)
        {
            AsyncClient.Send(data + "\n");
        }

        public void SendNewAddressBookEntry(AddressBookEntry e)
        {
            string data = string.Format("NewEntry[bn={0},name={1},surname={2},phone={3}]", "", e.Name, e.Surname, e.PhoneNumber);
            Send(data);
        }

        public void SendNewAddressBook(string name)
        {
            string data = string.Format("NewAddressBook[{0}]", name);
            Send(data);
        }

        private void DispatchQueue()
        {
            while (AsyncClient.messageQueue.Count > 0)
            {
                if (AsyncClient.messageQueue.TryDequeue(out string message))
                {
                    string cmd = message.Substring(0, message.IndexOf("["));
                    string args = message.Substring(message.IndexOf("[") + 1, message.IndexOf("]"));
                    Dictionary<string, string> data = ParseData(args);
                    if (cmd == "NewAddressBook")
                    {
                        if (data.ContainsKey("result"))
                        {
                            if (data.TryGetValue("result", out string val))
                                if (val == "succeeded")
                                {
                                    data.TryGetValue("name", out string name);
                                    MessageBox.Show("Created new address book: " + name);
                                    Instances.Books.Add(name);
                                }
                                else
                                {
                                    MessageBox.Show("Failed to create new address book");
                                }
                        }

                    }
                }
            }
        }

        private Dictionary<string, string> ParseData(string rawData)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach (string s in rawData.Split(','))
            {
                string[] parts = s.Split('=');
                data.Add(parts[0], parts[1]);
            }

            return data;
        }

    }
}
