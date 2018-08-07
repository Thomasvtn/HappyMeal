using System;
using System.IO;
using System.Security.Cryptography;
using HappyMeal_v3.Extensions;
using System.Text;

namespace HappyMeal_v3.Services
{
    public class DataBase : CryptClass
    {
        private byte[] _iv;
        private byte[] _key;

        private DataBase()
        {
            TripleDESCryptoServiceProvider TDES = new TripleDESCryptoServiceProvider();

            _iv = TDES.IV;
            _key = TDES.Key;
        }

        public static DataBase Instanciate = new DataBase();

        public bool AccountWriting(string path, string pseudo, string password, string email)
        {
            string[] strArray = File.ReadAllLines(path);

            foreach (var e in strArray)
            {
                var split = e.Split(';');

                if (split[0] == email)
                    return false;
            }

            string newuser = email + ";" + password + ";" + pseudo + "\n";

            File.AppendAllText(path, newuser);

            return true;
        }

        public bool Verifie(string path, string password, string email)
        {
            string[] strArray = File.ReadAllLines(path);

            var split = strArray[0].Split(';');

            if (password == split[1])
                return true;

            return false;
        }

        public string PseudoRecup(string path, string email)
        {
            string[] strArray = File.ReadAllLines(path);

            foreach (var e in strArray)
            {
                var split = e.Split(';');

                if (split[0] == email)
                {
                    return split[2];
                }
            }

            return null;
        }

    }
}
