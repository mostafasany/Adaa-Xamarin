﻿using System;
using AdaaMobile.Helpers;
using AdaaMobile.iOS.Helpers;
using Xamarin.Forms;
using System.Text;

[assembly: Dependency(typeof(CryptoGraphyService))]

namespace AdaaMobile.iOS.Helpers
{
    public class CryptoGraphyService : ICryptoGraphyService
    {
        public string Encrypt(string toEncrypt)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            // Get the key from config file
            string key = "MOEWPasswordMOEWPassword";

            keyArray = UTF8Encoding.UTF8.GetBytes(key);

            System.Security.Cryptography.TripleDESCryptoServiceProvider tdes = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. We choose ECB(Electronic code Book)
            tdes.Mode = System.Security.Cryptography.CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

            System.Security.Cryptography.ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray = cTransform.TransformFinalBlock
                    (toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
    }
}