using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RubricaXMLViewer.AddressBook.Data.Network
{

    public static class AsyncClient
    {
        private const int remotePort = 8192;
        private const string remoteAddress = "127.0.0.1";

        private static ManualResetEvent connectionDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        private static Socket client;

        private static string response = string.Empty;

        public static void StartClient()
        {
            IPAddress ip = IPAddress.Parse(remoteAddress);
            IPEndPoint remote = new IPEndPoint(ip, remotePort);
            client = new Socket(remote.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            client.BeginConnect(remote, new AsyncCallback(ConnectCallback), client);
            connectionDone.WaitOne();

        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndConnect(ar);
            connectionDone.Set();
        }

        public static void Send(string data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            client.BeginSend(bytes, 0, bytes.Length, 0, new AsyncCallback(SendCallback), client);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                int bytesSent = client.EndSend(ar);
                sendDone.Set();
            }
            catch (Exception)
            {

            }
        }

        public static void Receive()
        {

        }
    }

    internal class StateObject
    {
        public Socket clientSocket;
        public const int BufferSize = 512;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
    }
}
