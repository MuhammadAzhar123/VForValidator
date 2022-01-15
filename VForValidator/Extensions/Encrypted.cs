using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace VForValidator.Extensions
{
    public class Encrypted : Attribute { }


    public static class EncryptionService
    {
        public static string ToEncrypt(this string value)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(value));
        }

        public static string ToDecrypt(this string value)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(value));
        }
        //public static string ToEncrypt(this string value)
        //{

        //    return Encrypt(value, true);
        // }

        //public static string ToDecrypt(this string value)
        //{
        //    return Decrypt(value, true);
        // }


        public static string Encrypt(string toencrypt, bool usinghash)
        {
            byte[] keyarray;
            byte[] toencryptarray = UTF8Encoding.UTF8.GetBytes(toencrypt);

            string Um = ConfigurationManager.AppSettings["securitykey"];

            if (usinghash)
            {
                MD5CryptoServiceProvider Crypto = new MD5CryptoServiceProvider();
                keyarray = Crypto.ComputeHash(UTF8Encoding.UTF8.GetBytes(Um));
                Crypto.Clear();
            }

            else
                keyarray = UTF8Encoding.UTF8.GetBytes(Um);
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = keyarray;
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = des.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toencryptarray, 0, toencryptarray.Length);
            des.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string todecrypt, bool usinghash)
        {
            byte[] keyarray;
            byte[] todecryptarray = Convert.FromBase64String(todecrypt);

            string um = ConfigurationManager.AppSettings["securitykey"];
            if (usinghash)
            {
                MD5CryptoServiceProvider Crypto = new MD5CryptoServiceProvider();
                keyarray = Crypto.ComputeHash(UTF8Encoding.UTF8.GetBytes(um));
                Crypto.Clear();

            }
            else
                keyarray = UTF8Encoding.UTF8.GetBytes(um);

            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = keyarray;
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.PKCS7;
            ICryptoTransform transform = des.CreateDecryptor();
            byte[] resultarray = transform.TransformFinalBlock(todecryptarray, 0, todecryptarray.Length);
            des.Clear();
            return UTF8Encoding.UTF8.GetString(resultarray);
        }


        public static string ToBase64(this string value)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(encoded);
        }


    }
}
