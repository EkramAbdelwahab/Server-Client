using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.Security.Cryptography;
using System.Net;
namespace tcpServer
{
   public class Program
    {
        private const int portNum = 55555;
        public string publicKey;
        public string publicprivatekey;
        TcpListener listener;
        NetworkStream networkStream;
        TcpClient handler;
        byte[] kiv = new byte[16];
        public Program()
        {
            listener = new TcpListener(IPAddress.Any, portNum);
            listener.Start();
            Console.WriteLine("Waiting for a connection...");
            handler = listener.AcceptTcpClient();
            networkStream = handler.GetStream();
        }

        static void Main(string[] args)
        {
            Program ob = new Program();
            ob.GenerateKeys();
            Console.WriteLine(ob.publicKey);
            Console.WriteLine(ob.publicprivatekey);
            ob.load_Kiv();
            while (true)
            {
                ob.Decrypt();
            }


            
           
        }

        private void GenerateKeys()
        {

            getPublicPrivateKey();

            //send public key to the server
            byte[] DataToSend = Encoding.UTF8.GetBytes(publicKey);
            networkStream.Write(DataToSend, 0, DataToSend.Length);
            networkStream.Flush();

        }
        private void load_Kiv()
        {  
            byte[] encreptedkiv = new byte[128];
            int bytesread = networkStream.Read(encreptedkiv, 0, 128);
            kiv = Dec(encreptedkiv, publicprivatekey);
            Console.WriteLine();
          //  Console.WriteLine("the symetric key: "+Encoding.UTF8.GetString(kiv));
            Console.WriteLine();

        }
        private void Decrypt()
        {

            //read encrepted Data
            byte[] encreptedData = new byte[handler.ReceiveBufferSize];
            int BytesRead = networkStream.Read(encreptedData, 0, (int)handler.ReceiveBufferSize);
            string str = Convert.ToBase64String(encreptedData, 0, BytesRead);
            Console.WriteLine("the Encrepted message : " + str);
            string decrypted = decrypt(str, kiv, kiv);
            Console.WriteLine("the decrepted message : " + decrypted);
        }
        //Decrept text or String
        public static string decrypt(string data, byte[] key, byte[] iv)
        {
            return Encoding.ASCII.GetString(Encryption.decrypt(Convert.FromBase64String(data), key, iv));
        }



        //generate public and private key
        public void getPublicPrivateKey()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            publicprivatekey = rsa.ToXmlString(true);
            publicKey = rsa.ToXmlString(false);
        }

        //decrept data with symetric algorithm
        public static byte[] Dec(byte[] data, string publicprivatekey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicprivatekey);
            return rsa.Decrypt(data, false);
        }
    }




    class Encryption
    {

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
