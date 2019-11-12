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

        public static string GetResponse()
        {
            return response;
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);
                connectionDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
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
                StateObject state = (StateObject)ar.AsyncState;
                Socket c = state.clientSocket;

                int bytesRead = c.EndReceive(ar);

                if (bytesRead > 0)
                {
                    string data = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);
                    state.sb.Append(data);
                    if (!data.Contains("\n"))
                    {
                        c.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                            new AsyncCallback(ReceiveCallback), state);
                    }
                    else
                    {
                        response = state.sb.ToString();
                        receiveDone.Set();
                    }
                }
                else
                {
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString().Replace("\n", "");
                    }
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
            Console.WriteLine(response);
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
