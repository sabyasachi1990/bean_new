using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace AppsWorld.Bean.WebAPI.Utils
{
    public class AESEncryptDecrypt
    {
        public static int keySize = 256;
        public static int ivSize = 128;
        public static int saltSize = 256;
        public static int iterations = 10;
        public static string secret_key = Ziraff.FrameWork.SingleTon.CommonConnection.SecretKey;//Utils.KeyVaultProperty.SecretKey;
        public static int saltCount = 32;
        public static int ivCount = 32;
        public static string DecryptStringAESNew(string cipherText)
        {
            try
            {
                var salt = HexToByteArray(cipherText.Substring(0, saltCount));
                var password = new Rfc2898DeriveBytes(secret_key, salt, iterations);
                var keybytes = password.GetBytes(keySize / 8);
                var iv = HexToByteArray(cipherText.Substring(saltCount, ivCount));
                var encrypted = Convert.FromBase64String(cipherText.Substring((saltCount + ivCount), cipherText.Length - (saltCount + ivCount)));
                var decriptedStr = DecryptStringFromBytes(encrypted, keybytes, iv);
                return decriptedStr;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            string plaintext = null;
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.KeySize = keySize;
                rijAlg.Key = key;
                rijAlg.IV = iv;
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                try
                {
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return plaintext;
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            byte[] encrypted;
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.KeySize = 256;
                rijAlg.Key = key;
                rijAlg.IV = iv;

                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }
        public static byte[] HexToByteArray(string hex)
        {
            hex = hex.Replace(" ", "").Replace("-", "");
            var numberChars = hex.Length;
            var bytes = new byte[numberChars / 2];
            for (var i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}