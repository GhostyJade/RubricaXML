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
        public static DataListener Instance { get; private set; } = new DataListener();

        private Thread thread;

        private IPEndPoint remoteAddress;
        private Socket sender;

        private const int PACKET_SIZE = 1024;

        private bool running = false;

        private DataListener()
        {
            thread = new Thread(new ThreadStart(() =>
            {
                while (running)
                {
                    Recieve();
                }
            }));
            Init();
        }

        private void Init()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            remoteAddress = new IPEndPoint(ip, 8192);
            sender = new Socket(remoteAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sender.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            MessageBox.Show(sender.Poll(-1, SelectMode.SelectWrite).ToString());
        }

        public void Connect()
        {
            try
            {
                running = true;
                sender.Connect(remoteAddress);
                thread.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error on opening connection to the jar process");
            }
        }

        public bool IsConnected()
        {
            return sender.Connected;
        }

        public void Recieve()
        {
            while (sender.Connected)
            {
                byte[] recievedData = new byte[PACKET_SIZE * 10];
                sender.Receive(recievedData);
                Process(recievedData);
            }
        }

        private void Process(byte[] data)
        {
            string message = Encoding.UTF8.GetString(data);
            if (message.StartsWith(""))
            {

            }
        }

        public void Send(string message)
        {
            //if (sender.Connected)
            //{
            
            int byteSent = sender.Send(Encoding.ASCII.GetBytes(message));
            
            MessageBox.Show(byteSent.ToString());
            //}
        }

        public void Disconnect()
        {
            running = false;
            thread.Join();
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }

        public void SendNewAddressBookEntry(AddressBookEntry e)
        {
            string data = "CmdSave ";
            data += string.Format("Name:{0},Surname:{1},PhoneNumber:{2}", e.Name, e.Surname, e.PhoneNumber);
            Send(data);
        }

    }
}
