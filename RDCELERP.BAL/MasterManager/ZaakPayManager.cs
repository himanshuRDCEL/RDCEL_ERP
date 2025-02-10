using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;



namespace RDCELERP.BAL.MasterManager
{
    public class ParamSanitizer
    {
        public static string sanitizeParam(string param)
        {

            String ret = null;
            if (param == null)
                return null;

            ret = param.Replace("[>><>(){}?&* ~`!#$%^=+|\\:'\";,\\x5D\\x5B]+", " ");

            return ret;
        }

        public static String SanitizeURLParam(String url)
        {

            if (url == null)
                return "";

            Match match = Regex.Match(url, "^(https?)://[-a-zA-Z0-9+&@#/%?=~_|!:,.;]*[-a-zA-Z0-9+&@#/%=~_|]", RegexOptions.IgnoreCase);

            if (match.Success)

                return url;
            else
                return "";

        }

    }


    public static class ChecksumCalculator
    {
        public static string toHex(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();


        }
        public static string GenerateCheckSum(string paremeters)
        {
            byte[] dataToEncryptByte = Encoding.UTF8.GetBytes(paremeters);
            HMACSHA256 hmacsha256 = new HMACSHA256();
            byte[] checksumvalue = hmacsha256.ComputeHash(dataToEncryptByte);
            String checksum = toHex(checksumvalue);
            return checksum;

        }

        public static string calculateChecksum(string secretkey, string allparamvalues)
        {

            byte[] dataToEncryptByte = Encoding.UTF8.GetBytes(allparamvalues);
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretkey);
            HMACSHA256 hmacsha256 = new HMACSHA256(keyBytes);
            byte[] checksumByte = hmacsha256.ComputeHash(dataToEncryptByte);
            String checksum = toHex(checksumByte);

            return checksum;
        }

        public static Boolean verifyChecksum(String secretKey, String allParamVauleExceptChecksum, String checksumReceived)
        {

            byte[] dataToEncryptByte = Encoding.UTF8.GetBytes(allParamVauleExceptChecksum);
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);
            HMACSHA256 hmacsha256 = new HMACSHA256(keyBytes);
            byte[] checksumCalculatedByte = hmacsha256.ComputeHash(dataToEncryptByte); ;
            String checksumCalculated = toHex(checksumCalculatedByte);

            if (checksumReceived.Equals(checksumCalculated))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static string getAllNotEmptyParamValue(HttpRequest Request)
        {
            String allNonEmptyParamValue = "";
            // System.Text.StringBuilder displayValues = new System.Text.StringBuilder();

            NameValueCollection postedValues = (NameValueCollection)Request.Form;
            String paramName;

            for (int i = 0; i < postedValues.AllKeys.Length; i++)
            {
                paramName = postedValues.AllKeys[i];

                String paramValue = ParamSanitizer.sanitizeParam(Request.Form[paramName]);
                //String paramValue = ParamSanitizer.sanitizeParam(Request.Form.Get(paramName));

                if (paramValue != null)
                {
                    allNonEmptyParamValue = allNonEmptyParamValue + "'" + paramValue + "'";

                }

            }

            return allNonEmptyParamValue;

        }
        public static string getAllNotEmptyParamValuemodified(NameValueCollection postedValues)
        {
            String allNonEmptyParamValue = "";
            // System.Text.StringBuilder displayValues = new System.Text.StringBuilder();

            // NameValueCollection postedValues = Request.Form;
            String paramName;

            for (int i = 0; i < postedValues.AllKeys.Length; i++)
            {
                paramName = postedValues.AllKeys[i];


                String paramValue = ParamSanitizer.sanitizeParam(paramName);

                if (paramValue != null)
                {
                    allNonEmptyParamValue = allNonEmptyParamValue + "'" + paramValue + "'";

                }

            }

            return allNonEmptyParamValue;

        }

        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }


        }
        public static string generateSignature(Dictionary<string, string> input)
        {
            return (getStringByParams(input));
        }

        public static string generateSignature(string input, string key)
        {
            try
            {
                validateGenerateCheckSumInput(key);
                StringBuilder stringBuilder = new StringBuilder(input);
                stringBuilder.Append("|");
                string text = generateRandomString(4);
                stringBuilder.Append(text);
                string hashedString = getHashedString(stringBuilder.ToString());
                hashedString += text;
                return encrypt(hashedString, key);
            }
            catch (Exception ex)
            {
                ShowException(ex);
                return null;
            }
        }

        public static bool verifySignature(Dictionary<string, string> input, string key, string CheckSum)
        {
            return verifySignature(getStringByParams(input), key, CheckSum);
        }

        public static bool verifySignature(string input, string key, string CheckSum)
        {
            try
            {
                validateVerifyCheckSumInput(CheckSum, key);
                string text = decrypt(CheckSum, key);
                if (text == null || text.Length < 4)
                {
                    return false;
                }

                string text2 = text.Substring(text.Length - 4, 4);
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(input);
                stringBuilder.Append("|");
                stringBuilder.Append(text2);
                return text.Equals(getHashedString(stringBuilder.ToString()) + text2);
            }
            catch (Exception ex)
            {
                ShowException(ex);
                return false;
            }
        }

        public static string encrypt(string input, string key)
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(input);
                MemoryStream memoryStream = new MemoryStream();
                Rijndael rijndael = Rijndael.Create();
                rijndael.Key = Encoding.ASCII.GetBytes(key);
                rijndael.IV = new byte[16]
                {
                    64, 64, 64, 64, 38, 38, 38, 38, 35, 35,
                    35, 35, 36, 36, 36, 36
                };
                rijndael.Mode = CipherMode.CBC;
                rijndael.Padding = PaddingMode.PKCS7;
                CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(bytes, 0, bytes.Length);
                cryptoStream.Close();
                return Convert.ToBase64String(memoryStream.ToArray());
            }
            catch (Exception ex)
            {
                ShowException(ex);
                return null;
            }
        }

        public static string decrypt(string input, string key)
        {
            try
            {
                byte[] array = Convert.FromBase64String(input);
                MemoryStream memoryStream = new MemoryStream();
                Rijndael rijndael = Rijndael.Create();
                rijndael.Key = Encoding.ASCII.GetBytes(key);
                rijndael.IV = new byte[16]
                {
                    64, 64, 64, 64, 38, 38, 38, 38, 35, 35,
                    35, 35, 36, 36, 36, 36
                };
                rijndael.Mode = CipherMode.CBC;
                rijndael.Padding = PaddingMode.PKCS7;
                CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(array, 0, array.Length);
                cryptoStream.Close();
                return Encoding.ASCII.GetString(memoryStream.ToArray());
            }
            catch (Exception ex)
            {
                ShowException(ex);
                return null;
            }
        }

        private static void validateGenerateCheckSumInput(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("Parameter cannot be null", "Specified key");
            }
        }

        private static void validateVerifyCheckSumInput(string checkSum, string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("Parameter cannot be null", "Specified key");
            }

            if (checkSum == null)
            {
                throw new ArgumentNullException("Parameter cannot be null", "Specified checkSum");
            }
        }

        private static string getStringByParams(Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                return "";
            }

            SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);
            StringBuilder stringBuilder = new StringBuilder("");
            foreach (KeyValuePair<string, string> item in sortedDictionary)
            {
                //This will create the checksum string against every parameter.
                string text = item.Key + "=" + item.Value;
                if (text == null)
                {
                    text = "";
                }

                stringBuilder.Append(text).Append("&");
            }

            return stringBuilder.ToString().Substring(0, stringBuilder.Length - 1);
        }

        private static string generateRandomString(int length)
        {
            if (length <= 0)
            {
                return "";
            }

            Random random = new Random((int)DateTime.Now.Ticks);
            StringBuilder stringBuilder = new StringBuilder("");
            for (int i = 0; i < length; i++)
            {
                int startIndex = random.Next("@#!abcdefghijklmonpqrstuvwxyz#@01234567890123456789#@ABCDEFGHIJKLMNOPQRSTUVWXYZ#@".Length);
                stringBuilder.Append("@#!abcdefghijklmonpqrstuvwxyz#@01234567890123456789#@ABCDEFGHIJKLMNOPQRSTUVWXYZ#@".Substring(startIndex, 1));
            }

            return stringBuilder.ToString();
        }

        private static string getHashedString(string inputValue)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(inputValue);
            SHA256Managed sHA256Managed = new SHA256Managed();
            byte[] array = sHA256Managed.ComputeHash(bytes);
            return BitConverter.ToString(array).Replace("-", "").ToLower();
        }

        private static void ShowException(Exception ex)
        {
            Console.WriteLine("Message : " + ex.Message + Environment.NewLine + "StackTrace : " + ex.StackTrace);
        }
        public static NameValueCollection ToNameValueCollection<TKey, TValue>(
    this IDictionary<TKey, TValue> dict)
        {
            var nameValueCollection = new NameValueCollection();

            foreach (var kvp in dict)
            {
                string value = null;
                if (kvp.Value != null)
                    value = kvp.Value.ToString();

                nameValueCollection.Add(kvp.Key.ToString(), value);
            }

            return nameValueCollection;
        }
        public static string GetSHAGenerated(string request, string secureSecret)
        {
            string hexHash = String.Empty;

            byte[] convertedHash = new byte[secureSecret.Length / 2];
            for (int i = 0; i < secureSecret.Length / 2; i++)
            {
                convertedHash[i] = (byte)int.Parse(secureSecret.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }


            using (HMACSHA256 hasher = new HMACSHA256(convertedHash))
            {
                byte[] hashValue = hasher.ComputeHash(Encoding.UTF8.GetBytes(request));
                foreach (byte b in hashValue)
                {
                    hexHash += b.ToString("X2");
                }
            }

            return hexHash;
        }
    }
}

