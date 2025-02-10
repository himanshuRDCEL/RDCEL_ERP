using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;



namespace UTC_Bridge.DocUpload.BAL.Common
{
    public class AES256encoding
    {
        public static string Encrypt(string plainText, string key,string IV)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(plainText);

                using (SymmetricAlgorithm crypt = Aes.Create())
                using (HashAlgorithm hash = MD5.Create())
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    crypt.Padding = PaddingMode.PKCS7;
                    crypt.Key = Encoding.UTF8.GetBytes(key);
                    // This is really only needed before you call CreateEncryptor the second time,
                    // since it starts out random.  But it's here just to show it exists.
                    crypt.IV = Encoding.UTF8.GetBytes(IV);

                    using (CryptoStream cryptoStream = new CryptoStream(
                        memoryStream, crypt.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytes, 0, bytes.Length);
                    }

                    string base64IV = Convert.ToBase64String(crypt.IV);
                    string base64Ciphertext = Convert.ToBase64String(memoryStream.ToArray());
                    byte[] decoded = Convert.FromBase64String(base64Ciphertext);
                    string resultAsHex = (BitConverter.ToString(decoded).Replace("-", string.Empty));
                    return resultAsHex;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error encrypting: " + e.Message);
            }
        }

        public static string Decrypt(string combinedString, string keyString, string Iv)
        {
            string plaintext = null;

            //for add Hex string to char '-' to 2, 4, 6 and so on
            combinedString = Regex.Replace(combinedString, ".{2}", "$0-");
            combinedString = combinedString.Remove(combinedString.Length - 1);
            int length = (combinedString.Length + 1) / 3;

            //for string to byte array
            byte[] encrypted = new byte[length];
            for (int i = 0; i < length; i++)
                encrypted[i] = Convert.ToByte(combinedString.Substring(3 * i, 2), 16);
            // Create AesManaged    
            using (AesManaged aes = new AesManaged())
            {
                HashAlgorithm hash = MD5.Create();
                aes.Key = Encoding.UTF8.GetBytes(keyString);
                aes.IV = Encoding.UTF8.GetBytes(Iv);
                aes.Padding = PaddingMode.PKCS7;
                // Create a decryptor    
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                // Create the streams used for decryption.    
                using (MemoryStream ms = new MemoryStream(encrypted))
                {
                    // Create crypto stream    
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream    
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }
        
        public static string toHex(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static string DecryptString(string cipherText, string keyString, string IV)
        {
            try
            {
                string EncryptionKey = keyString;
                cipherText = cipherText.Replace(" ", "+");

                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes aes = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
            });

                    aes.Key = Encoding.UTF8.GetBytes(keyString);
                    aes.IV = Encoding.UTF8.GetBytes(IV);
                    aes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream ms = new MemoryStream(cipherBytes))
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (StreamReader reader = new StreamReader(cs))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (FormatException ex)
            {
                // Handle the Base64 format exception
                Console.WriteLine("Invalid Base64 string: " + ex.Message);
                return null; // or throw an exception, depending on your needs
            }
            catch (Exception ex)
            {
                // Handle other exceptions that may occur during decryption
                Console.WriteLine("Decryption error: " + ex.Message);
                return null; // or throw an exception, depending on your needs
            }
        }



        //public static string DecryptString(string cipherText, string keyString, string IV)
        //{
        //    string plaintext = null;
        //    string EncryptionKey = keyString; // "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //    cipherText = cipherText.Replace(" ", "+");
        //    byte[] cipherBytes = Convert.FromBase64String(cipherText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
        //    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        //});
        //        using (AesManaged aes = new AesManaged())
        //        {
        //            HashAlgorithm hash = MD5.Create();
        //            aes.Key = Encoding.UTF8.GetBytes(keyString);
        //            aes.IV = Encoding.UTF8.GetBytes(IV);
        //            aes.Padding = PaddingMode.PKCS7;
        //            // Create a decryptor    
        //            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        //            // Create the streams used for decryption.    
        //            using (MemoryStream ms = new MemoryStream(cipherBytes))
        //            {
        //                // Create crypto stream    
        //                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
        //                {
        //                    // Read crypto stream    
        //                    using (StreamReader reader = new StreamReader(cs))
        //                        plaintext = reader.ReadToEnd();
        //                }

                         
        //            }
        //        }
        //        return plaintext;
        //    }
        //}


        public static string EncryptString(string encryptString, string keyString,string IV)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(encryptString);

            using (SymmetricAlgorithm crypt = Aes.Create())
            using (HashAlgorithm hash = MD5.Create())
            using (MemoryStream memoryStream = new MemoryStream())
            {
                crypt.Padding = PaddingMode.PKCS7;
                crypt.Key = Encoding.UTF8.GetBytes(keyString);
                // This is really only needed before you call CreateEncryptor the second time,
                // since it starts out random.  But it's here just to show it exists.
                crypt.IV = Encoding.UTF8.GetBytes(IV);

                using (CryptoStream cryptoStream = new CryptoStream(
                    memoryStream, crypt.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(bytes, 0, bytes.Length);
                }

                string base64IV = Convert.ToBase64String(crypt.IV);
                string base64Ciphertext = Convert.ToBase64String(memoryStream.ToArray());
                byte[] decoded = Convert.FromBase64String(base64Ciphertext);
                string resultAsHex = (BitConverter.ToString(decoded).Replace("-", string.Empty));
                return base64Ciphertext;
            }
        }

        public static string DecryptStringne(string cipherText, string keyString)
        {
            string EncryptionKey = keyString; // "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }



}
