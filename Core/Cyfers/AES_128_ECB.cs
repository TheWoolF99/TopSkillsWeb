using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Core
{
    public class AES_128_ECB
    {
        private static string GetSecretKey()
        {
            string str = "kB7isBKMVmhQMgLWlXz9iQkjNVnPB0Wh";
            return Regex.Replace(str, @"\s+", String.Empty);
        }

        public static string Encrypt_AES_128_ECB(string plainText, string? Key = null)
        {
            // Convert the plain text to a byte array
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            // Create a new instance of AES encryption
            using (Aes aes = Aes.Create())
            {
                // Set the encryption key and mode
                aes.Key = Encoding.UTF8.GetBytes(Key?? GetSecretKey());
                aes.Mode = CipherMode.ECB;

                // Create an encryptor to perform the encryption
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                // Encrypt the plain text
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                // Convert the encrypted bytes to a base64 string
                string encryptedText = Convert.ToBase64String(encryptedBytes);
                return encryptedText;

            }
        }


        public static string Decrypt_AES_128_ECB(string plainText, string? Key = null)
        {
            if(!Convert.TryFromBase64String(plainText,new Span<byte>(new byte[plainText.Length]), out int bytesParsed))
            {
                return plainText;
            }

            // Convert the plain text to a byte array
            //byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] decryptedBytes = Convert.FromBase64String(plainText);


            // Create a new instance of AES encryption
            using (Aes aes = Aes.Create())
            {
                // Set the encryption key and mode
                aes.Key = Encoding.UTF8.GetBytes(Key??=GetSecretKey());
                aes.Mode = CipherMode.ECB;

                // Create an encryptor to perform the encryption
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                //using (MemoryStream _memoryStream = new MemoryStream(_originalString))
                //{
                //    using (CryptoStream _cryptoStream = new CryptoStream(_memoryStream, decryptor, CryptoStreamMode.Read))
                //    {
                //        using (StreamReader _streamReader = new StreamReader(_cryptoStream))
                //        {
                //            plaintext = _streamReader.ReadToEnd();
                //        }
                //    }
                //}

                // Encrypt the plain text
                byte[] decryptBytes = decryptor.TransformFinalBlock(decryptedBytes, 0, decryptedBytes.Length);


                string decryptText = Convert.ToBase64String(decryptBytes); //Encoding.Unicode.GetString(encryptedBytes);
                string res = UTF8Encoding.UTF8.GetString(decryptBytes);
                Console.WriteLine("Хэш-" + plainText + " | " + "Text-" + decryptText + " | UTF8 - " + res);
                return res;


                //// Convert the encrypted bytes to a base64 string
                //string encryptedText = Convert.ToBase64String(encryptedBytes);
                //return encryptedText;
            }
        }
    }
}
