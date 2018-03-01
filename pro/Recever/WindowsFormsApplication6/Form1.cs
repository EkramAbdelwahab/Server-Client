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
        byte[] kiv = new byte[16];
        public Form1()
        {
            InitializeComponent();
        }

        //generate pubblic and private keys
        private void GenerateKeys_Click(object sender, EventArgs e)
        {
            RSAencryption.getPublicPrivateKey("D:\\publicprivate", "D:\\public");
        }


        private void button2_Click(object sender, EventArgs e)
        {    
            FileStream key = new FileStream("D:\\encreptedKiv.txt", FileMode.Open, FileAccess.Read);
                MemoryStream ms = new MemoryStream();
                key.CopyTo(ms);
                key.Close();
                kiv = RSAencryption.Dec(ms.ToArray(), "D:\\publicprivate");
                FileStream fs = new FileStream("D:\\encreptedData.txt", FileMode.Open, FileAccess.Read);
                MemoryStream mm = new MemoryStream();
                fs.CopyTo(mm);
                fs.Close(); 
            string decrypted = decrypt(Encoding.ASCII.GetString(mm.ToArray()), kiv, kiv);
            textBox1.Text = decrypted;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FileStream key = new FileStream("D:\\encreptedKiv.txt", FileMode.Open, FileAccess.Read);
            MemoryStream ms = new MemoryStream();
            key.CopyTo(ms);
            key.Close();
            kiv = RSAencryption.Dec(ms.ToArray(), "D:\\publicprivate");

            Decrypt("D:\\file2.txt", "D:\\file3.txt", kiv, kiv);
        }

        //Decrept text or String
        public static string decrypt(string data, byte[] key, byte[] iv)
        {
                return Encoding.ASCII.GetString(Encryption.decrypt(Convert.FromBase64String(data), key, iv));

        }

         //Decrept File
          public static void Decrypt(string filein, string fileout, byte[] kiv, byte[] iv)
          {
              FileStream fsIn = new FileStream(filein, FileMode.Open, FileAccess.Read);
              var mem = new MemoryStream();
              fsIn.CopyTo(mem);
              byte[] Out = Encryption.decrypt(mem.ToArray(), kiv, iv);
              using (Stream file = File.Create(fileout))
              {
                  file.Write(Out, 0, Out.Length);
              }
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

      public class RSAencryption
    {
        public static void getPublicPrivateKey(string publicprivatekey, string publickey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            StreamWriter sw = new StreamWriter(publicprivatekey);
            string publicprivateXml = rsa.ToXmlString(true);
            sw.Write(publicprivateXml);
            sw.Close();
            sw = new StreamWriter(publickey);
            string publicXml = rsa.ToXmlString(false);
            sw.Write(publicXml);
            sw.Close();
            //return publicprivateXml;
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
    }
}