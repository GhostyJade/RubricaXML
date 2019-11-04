using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkTests
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                IPAddress ip = IPAddress.Parse("127.0.0.1");
                IPEndPoint p = new IPEndPoint(ip, 8192);
                Socket sender = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {

                    // Connect Socket to the remote  
                    // endpoint using method Connect() 
                    sender.Connect(p);

                    // We print EndPoint information  
                    // that we are connected 
                    Console.WriteLine("Socket connected to -> {0} ",
                                  sender.RemoteEndPoint.ToString());

                    // Creation of messagge that 
                    // we will send to Server
                    string data = "CmdSave " + string.Format("Name:{0},Surname:{1},PhoneNumber:{2}", "Name", "Sn", "3221028228");
                    byte[] messageSent = Encoding.ASCII.GetBytes(data);
                    int byteSent = sender.Send(messageSent);

                    // Data buffer 
                    //byte[] messageReceived = new byte[1024];

                    // We receive the messagge using  
                    // the method Receive(). This  
                    // method returns number of bytes 
                    // received, that we'll use to  
                    // convert them to string 
                    //int byteRecv = sender.Receive(messageReceived);
                    //Console.WriteLine("Message from Server -> {0}",
                    //     Encoding.ASCII.GetString(messageReceived,
                    //                                0, byteRecv));

                    // Close Socket using  
                    // the method Close() 
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }

                // Manage of Socket's Exceptions 
                catch (ArgumentNullException ane)
                {

                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }

                catch (SocketException se)
                {

                    Console.WriteLine("SocketException : {0}", se.ToString());
                }

                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
