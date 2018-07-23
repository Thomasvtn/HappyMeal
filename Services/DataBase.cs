using System.IO;

namespace HappyMeal_v3.Services
{
    public class DataBase
    {
        public static bool Verifie(string path, string pseudo, string password, string mail)
        {
            string[] log = File.ReadAllLines(path);
            string actual = mail + ";" + pseudo + ";" + password;
            int length = log.Length;
            int i = 0;

            while(log[i] != actual)
            {
                i += 1;

                if (i == length)
                    return false;
            }

            return i != length;
        }

    }
}
