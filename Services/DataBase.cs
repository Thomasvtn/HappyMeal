using System.IO;

namespace HappyMeal_v3.Services
{
    public class DataBase
    {
        public static bool Verifie(string path, string pseudo, string password, string email)
        {
            /*string[] log = File.ReadAllLines(path);
            string actual = mail + ";" + pseudo + ";" + password;
            int length = log.Length;
            int i = 0;

            while(log[i] != actual)
            {
                if (i == length)
                    return false;

                i += 1;
            }

            return i != length;*/

            var logs = File.ReadAllLines(path);
            foreach(var log in logs)
            {
                var split = log.Split(';');
                if (split.Length != 3)
                    continue;

                if (split[0] == email && split[2] == password)
                    return true;
            }
            return false;
        }

    }
}
