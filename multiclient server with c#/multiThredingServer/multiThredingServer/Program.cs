using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections;
using System.Threading;
using System.Security.Cryptography;
namespace tcpServer
{

    public class SynchronousSocketListener
    {

        private const int portNum = 55555;
        private static ArrayList ClientSockets;
  
        public static int ClientNum = 0;




        public static int Main(String[] args)
        {
           
            ClientSockets = new ArrayList();


            TcpListener listener = new TcpListener(IPAddress.Any, portNum);
            try
            {
                // Start listening for connections.
                listener.Start();
                Console.WriteLine("Waiting for a connection...");
                while (true)
                {
                    TcpClient Server = listener.AcceptTcpClient();
                        Console.WriteLine("Client#{0} accepted!", ++ClientNum);
                            int i = ClientSockets.Add(new ClientHandler(Server));
                            ((ClientHandler)ClientSockets[i]).Start();

                }


            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\n press enter to continue...");
            Console.Read();
            return 0;

        }
        public int getClientNum()
        {
            return ClientNum;
        }


    }

    class ClientHandler
    {

        TcpClient ClientSocket;
        bool ContinueProcess = false;
        Thread ClientThread;
  
        byte[] kiv = new byte[16];

        public ClientHandler(TcpClient ClientSocket)
        {
            this.ClientSocket = ClientSocket;
        }

        public void Start()
        {
            ContinueProcess = true;
            ClientThread = new Thread(new ThreadStart(Process));
            ClientThread.Start();
        }

        private void Process()
        {
            SynchronousSocketListener obj = new SynchronousSocketListener();
            int num = obj.getClientNum();

            if (ClientSocket != null)
            {
                NetworkStream networkStream = ClientSocket.GetStream();
               

                //send public key to the server
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                StreamReader sr = new StreamReader("publickey");
                string publickeyXml = sr.ReadToEnd();
                byte[] DataToSend = Encoding.UTF8.GetBytes(publickeyXml);
                networkStream.Write(DataToSend, 0, DataToSend.Length);
                networkStream.Flush();

                //read encrepted  symetric key from client
                byte[] encreptedkiv = new byte[128];
                int bytesread = networkStream.Read(encreptedkiv, 0, 128);
                kiv = Decryption.Dec(encreptedkiv, "publicprivatekey");
                Console.WriteLine();
                


                while (ContinueProcess)
                {
                    try
                    {
                        //read encrepted Data
                        byte[] encreptedData = new byte[ClientSocket.ReceiveBufferSize];
                        int BytesRead = networkStream.Read(encreptedData, 0, (int)ClientSocket.ReceiveBufferSize);
                        string str = Convert.ToBase64String(encreptedData, 0, BytesRead);
                        Console.WriteLine("the Encrepted message from client "+num+" : " + str);
                        string decrypted =  Decryption.decrypt(str, kiv, kiv);
                        Console.WriteLine("the decrepted message from client " + num + " : " + decrypted);
                        Console.WriteLine();
                        Console.WriteLine("enter message to client " + num + " : ");
                        string str1 = "";

                        str1 = Console.ReadLine();
                        string encoded = Decryption.encrypt(str1, kiv, kiv);
                        byte[] DataToSend1 = Convert.FromBase64String(encoded);
                        networkStream.Write(DataToSend1, 0, DataToSend1.Length);
                        networkStream.Flush();
                        Console.WriteLine();

                    }
                    catch (IOException) { } // Timeout
                    catch (SocketException)
                    {
                        Console.WriteLine("Conection is broken!");
                        break;
                    }

                    
                } // while ( ContinueProcess )
                networkStream.Close();
                ClientSocket.Close();
            }
        }  // Process()


    } // class ClientHandler 



    class Decryption
    {

        public static string encrypt(string data, byte[] key, byte[] iv)
        {

            return Convert.ToBase64String(Encrypt(Encoding.ASCII.GetBytes(data), key, iv));
        }

        public static string decrypt(string data, byte[] key, byte[] iv)
        {
            return Encoding.ASCII.GetString(decrypt(Convert.FromBase64String(data), key, iv));
        }

        public static byte[] Dec(byte[] data, string publicprivatekey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            StreamReader sr = new StreamReader(publicprivatekey);
            string publicprivateXml = sr.ReadToEnd();
            rsa.FromXmlString(publicprivateXml);
            sr.Close();
            return rsa.Decrypt(data, false);
        }

        public static byte[] Encrypt(byte[] data, byte[] key,  byte[] iv)
        {
            using (Aes algorithm = Aes.Create())
            {
                algorithm.Padding = PaddingMode.PKCS7;
                algorithm.Mode = CipherMode.CBC;
                algorithm.GenerateIV();
                //iv = algorithm.IV;
                algorithm.GenerateKey();
                //key = algorithm.Key;
                using (ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv))
                    return crypt(data, encryptor);
            }

        }

        public static byte[] decrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (Aes algorithm = Aes.Create())
            {
                algorithm.Padding = PaddingMode.PKCS7;
                algorithm.Mode = CipherMode.CBC;
                using (ICryptoTransform decryptor = algorithm.CreateDecryptor(key, iv))
                    return crypt(data, decryptor);
            }
        }
        public static byte[] crypt(byte[] data, ICryptoTransform cryptor)
        {
            MemoryStream m = new MemoryStream();
            using (CryptoStream c = new CryptoStream(m, cryptor, CryptoStreamMode.Write))
            {

                c.Write(data, 0, data.Length);
                c.FlushFinalBlock();

            }

            return m.ToArray();

        }

    }
}