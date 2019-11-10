using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace RubricaXMLViewer.AddressBook.Data.Network
{

    internal static class AsyncClient
    {
        public static ConcurrentQueue<string> messageQueue { get; } = new ConcurrentQueue<string>();

        private const int remotePort = 8192;
        private const string remoteAddress = "127.0.0.1";

        private static ManualResetEvent connectionDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        public static ManualResetEvent receiveDone = new ManualResetEvent(false);

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
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket   
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket c = state.clientSocket;

                // Read data from the remote device.  
                int bytesRead = c.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data.  
                    c.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    // All the data has arrived; put it in response.  
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                        messageQueue.Enqueue(response);
                    }
                    // Signal that all bytes have been received.  
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void Receive()
        {
            try
            {
                StateObject state = new StateObject();
                state.clientSocket = client;
                 
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void Close()
        {
            client.Shutdown(SocketShutdown.Both);
            client.Close();
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
