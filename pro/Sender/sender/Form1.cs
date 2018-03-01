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

namespace WindowsFormsApplication6
{
    public partial class Form1 : Form
    {
     // byte[] kiv = new byte[16];
        byte[] kiv = { 145, 12, 32, 245, 98, 132, 98, 214, 6, 77, 131, 44, 221, 3, 9, 50 };
        public Form1()
        {
            InitializeComponent();
            
        }

        private void encryp_Click(object sender, EventArgs e)
        {
            string x = textBox3.Text;
            string encrypted = encrypt(x, kiv, ref kiv);
            byte[] data = Encoding.UTF8.GetBytes(encrypted);
            using (Stream file = File.Create("D:\\public\\encreptedData.txt"))
            {
                file.Write(data, 0, data.Length);
            }
            textBox3.Clear();
        }

        public static string encrypt(string data, byte[] key, ref byte[] iv)
        {

            return Convert.ToBase64String(Encryption.encrypt(Encoding.UTF8.GetBytes(data), key, ref iv));
        }


        private void LoadPublicKey_Click(object sender, EventArgs e)
        {
            //RandomNumberGenerator.Create().GetBytes(kiv);
         //   byte[] kiv = {145, 12, 32, 245, 98, 132, 98, 214, 6, 77, 131, 44, 221, 3, 9, 50 };
           // byte[] encrKiv = Enc(kiv, "D:\\public");
            byte[] encrKiv = Enc(kiv, "asd");
            using (Stream file = File.Create("D:\\encreptedKiv.txt"))
           {
               file.Write(encrKiv, 0, encrKiv.Length);
           }
        }

        private void EncryptFile_Click(object sender, EventArgs e)
        {
            Encrypt("D:\\file1.txt", "D:\\file2.txt", kiv, ref kiv);
        }

          public static void Encrypt(string filein, string fileout, byte[] kiv, ref byte[] iv)
        {
            FileStream fsIn = new FileStream(filein, FileMode.Open, FileAccess.Read);
            var mem = new MemoryStream();
            fsIn.CopyTo(mem);
            byte[] Out = Encryption.encrypt(mem.ToArray(), kiv, ref iv);
            using (Stream file = File.Create(fileout))
            {
                file.Write(Out, 0, Out.Length);
            }

        }


       

        public static byte[] Enc(byte[] data, string publickey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            StreamReader sr = new StreamReader(publickey);
            string publickeyXml = sr.ReadToEnd();
            rsa.FromXmlString(publickeyXml);
            sr.Close();
            return rsa.Encrypt(data, false);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }





    class Encryption
    {
        public static byte[] encrypt(byte[] data, byte[] key, ref byte[] iv)
        {
            using (Aes algorithm = Aes.Create())
            {
                algorithm.Padding = PaddingMode.PKCS7;
                algorithm.Mode = CipherMode.CBC;
                algorithm.GenerateIV();
              
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