using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.Net;
namespace tcpServer
{
    class Program
    {
        private const int portNum = 10116;
        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Any,portNum);
            listener.Start();
            Console.WriteLine("Waiting for a connection...");
            TcpClient handler = listener.AcceptTcpClient();
            NetworkStream clientStream = handler.GetStream();
            ASCIIEncoding encoder = new ASCIIEncoding();
            try
            {
                while (true)
                {

                    byte[] message = new byte[handler.ReceiveBufferSize];
                    int numOfBytes = clientStream.Read(message, 0, message.Length);
                    string str = encoder.GetString(message, 0, numOfBytes);
                    Console.WriteLine("Data from client:" + str);
                    string s2 = "server:" + str;
                    byte[] reply = encoder.GetBytes(s2);
                    clientStream.Write(reply, 0, reply.Length);



                }
          
             }catch (System.IO.IOException)
               {
                Console.WriteLine("client not available!");
               }
        }
    }



    //Class to handle each client request separatly
    public class handleClinet
    {
        TcpClient clientSocket;
        string clNo;
        public void startClient(TcpClient inClientSocket, string clineNo)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }
        private void doChat()
        {
            int requestCount = 0;
            byte[] bytesFrom = new byte[10025];
            string dataFromClient = null;
            Byte[] sendBytes = null;
            string serverResponse = null;
            string rCount = null;
            requestCount = 0;

            while ((true))
            {
                try
                {
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    Console.WriteLine(" >> " + "From client-" + clNo + dataFromClient);

                    rCount = Convert.ToString(requestCount);
                    serverResponse = "Server to clinet(" + clNo + ") " + rCount;
                    sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                    Console.WriteLine(" >> " + serverResponse);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" >> " + ex.ToString());
                }
            }
        }
    } 
}
