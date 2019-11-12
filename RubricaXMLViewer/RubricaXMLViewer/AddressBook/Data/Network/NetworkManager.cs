using RubricaXMLViewer.AddressBook.Utils;
using System.Collections.Generic;
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
            WaitForMessages(out string msg);
        }

        private void WaitForMessages(out string msg)
        {
            AsyncClient.Receive(out msg);
            AsyncClient.receiveDone.WaitOne();
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

        private void ProcessMessage()
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
