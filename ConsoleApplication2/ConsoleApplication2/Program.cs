using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Net.Sockets;
using System.Threading;
public class AsynchIOServer
{
    static TcpListener tcpListener = new TcpListener(10);


    

    static void Listeners()
    {

        byte[] kiv = { 145, 12, 32, 245, 98, 132, 98, 214, 6, 77, 131, 44, 3, 9, 50, 0 };
        byte[] iv = { 15, 122, 132, 5, 93, 198, 44, 31, 9, 39, 241, 49, 250, 188, 80, 7 };

        Socket socketForClient = tcpListener.AcceptSocket();
        if (socketForClient.Connected)
        {
            //Console.WriteLine("Client now connected to server.");
            Console.WriteLine("Client:"+socketForClient.RemoteEndPoint+" now connected to server.");
            NetworkStream networkStream = new NetworkStream(socketForClient);
            System.IO.StreamWriter streamWriter =
            new System.IO.StreamWriter(networkStream);
            System.IO.StreamReader streamReader =
            new System.IO.StreamReader(networkStream);

            ////here we send message to client
            //Console.WriteLine("type your message to be recieved by client:");
            //string theString = Console.ReadLine();
            //streamWriter.WriteLine(theString);
            ////Console.WriteLine(theString);
            //streamWriter.Flush();

            //while (true)
            //{
            //here we recieve client's text if any.
            while (true)
            {
                string theString = streamReader.ReadLine();
                string decoded = Class1.Decrypt1(theString,kiv,ref iv);
                Console.WriteLine("Encrypted Message recieved by client:" + decoded);
                Console.WriteLine("Original Message recieved by client:" + theString);
                if (theString == "exit")
                    break;
               
                //here we send message to the client to do chat with
                    //*****************************************************************************
                    string str = "";
                    
                    str = Console.ReadLine();
                    string encoded = Class1.Encrypt1(str, kiv, ref iv);
                    streamWriter.WriteLine(encoded);                 
                    Console.WriteLine("type:");                    
                    streamWriter.Flush();



                    //******************************************************************************
                
            }
            
            streamReader.Close();
            networkStream.Close();
            streamWriter.Close();
            //}

        }
        socketForClient.Close();
        Console.WriteLine("Press any key to exit from server program");
        Console.ReadKey();
    }
   
    public static void Main()
    {
        //TcpListener tcpListener = new TcpListener(10);
        tcpListener.Start();
        Console.WriteLine("************This is Server program************");
        Console.WriteLine("How many clients are going to connect to this server?:");
        int numberOfClientsYouNeedToConnect =int.Parse( Console.ReadLine());
        for (int i = 0; i < numberOfClientsYouNeedToConnect; i++)
        {
            Thread newThread = new Thread(new ThreadStart(Listeners));
            newThread.Start();
        }
    }


    class Class1
    {
        // Encrypt function
        public static byte[] Encrypt(byte[] data, byte[] key, ref byte[] iv)
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
                    return Crypt(data, encryptor);
            }

        }


        // Decrypt function
        public static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (Aes algorithm = Aes.Create())
            {
                algorithm.Padding = PaddingMode.PKCS7;
                algorithm.Mode = CipherMode.CBC;
                using (ICryptoTransform decryptor = algorithm.CreateDecryptor(key, iv))
                    return Crypt(data, decryptor);
            }
        }

        //Crypt function
        private static byte[] Crypt(byte[] data, ICryptoTransform cryptor)
        {
            MemoryStream m = new MemoryStream();
            using (CryptoStream c = new CryptoStream(m, cryptor, CryptoStreamMode.Write))
            {
                c.Write(data, 0, data.Length);
                c.FlushFinalBlock();

            }
            return m.ToArray();
        }


        // overlap of Encrypt
        public static string Encrypt1(string data, byte[] key, ref byte[] iv)
        {
            return Convert.ToBase64String(
                Class1.Encrypt(Encoding.UTF8.GetBytes(data), key, ref iv));
        }

        // overlap of Decrypt
        public static string Decrypt1(string data, byte[] key, ref byte[] iv)
        {
            return Encoding.UTF8.GetString(
                Class1.Decrypt(Convert.FromBase64String(data), key, iv));
        }
    }

}
