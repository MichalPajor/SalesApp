using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SalesApp.Fiscal
{
    public static class TCPConnection
    {
        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        public static ManualResetEvent sendDone = new ManualResetEvent(false);
        public static ManualResetEvent receiveDone = new ManualResetEvent(false);

        // The response from the remote device.  
        private static String response = String.Empty;
        public static Socket clientSocket;
        public static void StartTCPClient(IPAddress ip, int port)
        {
            IPEndPoint remoteEP = new IPEndPoint(ip, port);

            // Create a TCP/IP socket.  
            clientSocket = new Socket(ip.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);
            clientSocket.SendTimeout = 5000;
            clientSocket.ReceiveTimeout = 5000;

            // Connect to the remote endpoint.  
            clientSocket.BeginConnect(remoteEP,
                new AsyncCallback(ConnectCallback), clientSocket);

        }
        public static bool SocketConnected()
        {
            bool part1 = clientSocket.Poll(1000, SelectMode.SelectRead);
            bool part2 = clientSocket.Available == 0;
            if (part1 && part2 || !clientSocket.Connected)
                return false;
            else
                return true;
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine();
            }
        }

        public static void Receive(Socket client)
        {
            try
            {
                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            if (clientSocket.Connected)
            {
                try
                {
                    // Retrieve the state object and the client socket
                    // from the asynchronous state object.  
                    StateObject state = (StateObject)ar.AsyncState;
                    Socket client = state.workSocket;

                    // Read data from the remote device.  
                    int bytesRead = client.EndReceive(ar);

                    if (bytesRead > 0)
                    {
                        // There might be more data, so store the data received so far.  
                        state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                        // Get the rest of the data.  
                        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                            new AsyncCallback(ReceiveCallback), state);
                        state.receive = new byte[bytesRead];
                        Array.Copy(state.buffer, state.receive, bytesRead);

                    }
                    else
                    {
                        // All the data has arrived; put it in response.  
                        if (state.sb.Length > 1)
                        {
                            response = state.sb.ToString();
                        }
                        // Signal that all bytes have been received.  
                        receiveDone.Set();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public static void Send(byte[] data)
        {
            // Begin sending the data to the remote device.  
            clientSocket.BeginSend(data, 0, data.Length, 0,
                new AsyncCallback(SendCallback), clientSocket);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                //Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.  
                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void CloseTCPConnection()
        {
            try
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch
            {
                //Console.WriteLine(ex.Message);
                return;
            }
        }

    }

    // State object for receiving data from remote device.
    public class StateObject
    {
        // Client socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 2048;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
        public byte[] receive;
    }
}

