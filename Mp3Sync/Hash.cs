using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;
using System.IO;

namespace Mp3Sync
{
    public static class Hash
    {
        private static MD5 md5 = new MD5CryptoServiceProvider();
        private static SHA1CryptoServiceProvider umSHA1 = new SHA1CryptoServiceProvider();

        public static long MD5processed = 0;
        public static long SHA1processed = 0;
        public static Stopwatch SHA1sw = new Stopwatch();
        public static Stopwatch MD5sw = new Stopwatch();
        public static string GetSAH1HashFromFile(string fileName)
        {
            try
            {
                SHA1sw.Start();
                FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                byte[] retVal = umSHA1.ComputeHash(file);
                file.Close();

                string st =  ConstructStringFromHashBytes(retVal);
                SHA1sw.Stop();
                ++SHA1processed;
                return st;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static string ConstructStringFromHashBytes(byte[] retVal)
        {
            MD5sw.Start();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                MD5sw.Start();
                FileStream file = new FileStream(fileName, FileMode.Open);
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                string st = ConstructStringFromHashBytes(retVal);
                MD5sw.Stop();
                ++MD5processed;
                return st;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
