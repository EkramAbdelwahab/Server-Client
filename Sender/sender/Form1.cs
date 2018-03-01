using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Net.Sockets;

namespace WindowsFormsApplication6
{
    public partial class Form1 : Form
    {
        private const int portNum = 55555;
        TcpClient tcpClient;
        NetworkStream networkStream;
        ASCIIEncoding encoder;
        string publicKey;
     // byte[] kiv = new byte[16];
        byte[] kiv = {145, 12, 32, 245, 98, 132, 98, 214, 6, 77, 131, 44, 221, 3, 9, 50 };
        public Form1()
        {
            InitializeComponent();
            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect("localhost", portNum);
                networkStream = tcpClient.GetStream();
                encoder = new ASCIIEncoding();
            }
            catch (SocketException)
            {
                Console.WriteLine("Sever not available!");
            }
            
        }

        private void encryp_Click(object sender, EventArgs e)
        {
            
                string plainText = textBox3.Text;
                string encrypted = encrypt(plainText, kiv, kiv);
                byte[] DataToSend =Convert.FromBase64String(encrypted);
                networkStream.Write(DataToSend, 0, DataToSend.Length);
                networkStream.Flush();
                textBox3.Clear();
           
        }

        public static string encrypt(string data, byte[] key,  byte[] iv)
        {

            return Convert.ToBase64String(Encryption.encrypt(Encoding.ASCII.GetBytes(data), key,  iv));
        }


        private void LoadPublicKey_Click(object sender, EventArgs e)
        {
            
            
            //read public key from server
                byte[] Key = new byte[tcpClient.ReceiveBufferSize];
                int BytesRead = networkStream.Read(Key, 0, (int)tcpClient.ReceiveBufferSize);
                 publicKey = Encoding.ASCII.GetString(Key, 0, BytesRead);
              
               
           //encrept the key of symetric algorithm with Asymetric algorithm
                byte[] encrKiv = Enc(kiv, publicKey);
                networkStream.Write(encrKiv, 0, encrKiv.Length);
                networkStream.Flush();
                LoadPublicKey.Enabled = false;
           
        }


       

        public static byte[] Enc(byte[] data, string publickey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publickey);
            return rsa.Encrypt(data, false);
        }

    }





    class Encryption
    {
        public static byte[] encrypt(byte[] data, byte[] key,  byte[] iv)
        {
            using (Aes algorithm = Aes.Create())
            {
                algorithm.Padding = PaddingMode.PKCS7;
                algorithm.Mode = CipherMode.CBC;
               
              
                using (ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv))
                    return crypt(data, encryptor);
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