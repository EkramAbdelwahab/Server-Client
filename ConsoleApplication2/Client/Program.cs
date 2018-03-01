using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Net.Sockets;
using System.Threading;
public class Client
{
    
    //private const int portNum = 10116;
    static public void Main(string[] Args)
    {

        byte[] kiv = { 145, 12, 32, 245, 98, 132, 98, 214, 6, 77, 131, 44, 3, 9, 50, 0 };
        byte[] iv = { 15, 122, 132, 5, 93, 198, 44, 31, 9, 39, 241, 49, 250, 188, 80, 7 };
        TcpClient socketForServer;
        try
        {
            
            socketForServer = new TcpClient("localHost", 10);
           
        }
        catch
        {
            Console.WriteLine(
            "Failed to connect to server at {0}:999", "localhost");
            return;
        }
       
        NetworkStream networkStream = socketForServer.GetStream();
        System.IO.StreamReader streamReader =
        new System.IO.StreamReader(networkStream);
        System.IO.StreamWriter streamWriter =
        new System.IO.StreamWriter(networkStream);
        Console.WriteLine("*******This is client program who is connected to localhost on port No:10*****");
        
        try
        {
            
            {
                ////////////////////////////////////////////////////////////////
        //        private void LoadPublicKey_Click(object sender, EventArgs e)
        //{
            
            
        //    //read public key from server
        //        byte[] Key = new byte[tcpClient.ReceiveBufferSize];
        //        int BytesRead = networkStream.Read(Key, 0, (int)tcpClient.ReceiveBufferSize);
        //         publicKey = Encoding.ASCII.GetString(Key, 0, BytesRead);
              
               
        //   //encrept the key of symetric algorithm with Asymetric algorithm
        //        byte[] encrKiv = Enc(kiv, publicKey);
        //        networkStream.Write(encrKiv, 0, encrKiv.Length);
        //        networkStream.Flush();
        //        LoadPublicKey.Enabled = false;
           
        //}
                
    ///////////////////////////////////////////////////////////////////////////

                Console.WriteLine("type:");
                string str = Console.ReadLine();
                while (str != "exit")
                {

                    string encoded = Class1.Encrypt1(str,kiv,ref iv);
                    streamWriter.WriteLine(encoded);
                    streamWriter.Flush();
                    //here we send message to the server to reply it to its message ^_^
                    //************************************************
                    string rep = "";
                    rep = streamReader.ReadLine();
                    string decoded = Class1.Decrypt1(rep, kiv, ref iv);
                    Console.WriteLine("Message recieved by server:" + rep);
                    Console.WriteLine("Original Message recieved by server:" + decoded);
                    streamWriter.Flush();
                    //*************************************************
                    
                    Console.WriteLine("type:");
                    str = Console.ReadLine();
                    
                }
                

                if (str == "exit")
                {
                    streamWriter.WriteLine(str);
                    streamWriter.Flush();
                   
                }

               
               
            }
        }
        catch
        {
            Console.WriteLine("Exception reading from Server");
        }
        // tidy up
        networkStream.Close();
        Console.WriteLine("Press any key to exit from client program");
        Console.ReadKey();
    }

    private static string GetData()
    {
        //Ack from sql server
        return "ack";
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