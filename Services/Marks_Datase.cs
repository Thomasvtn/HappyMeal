using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace HappyMeal_v3.Services
{
    public class Marks_Datase
    {

        public static void WriteMark(int mark, string food)
        {
            string json_dico = File.ReadAllText("C:/Users/adelamare/Downloads/Thomas aka le stagiaire/HappyMeal_v3/HappyMeal_v3/Marks.txt");

            Dictionary <string, int> dico = JsonConvert.DeserializeObject<Dictionary<string, int>>(json_dico);

            if (dico == null)
            {
                Dictionary<string, int> dico2 = new Dictionary<string, int>();
                dico2.Add(food, mark);
                string json = JsonConvert.SerializeObject(dico2, Formatting.Indented);

                File.Delete("C:/Users/adelamare/Downloads/Thomas aka le stagiaire/HappyMeal_v3/HappyMeal_v3/Marks.txt");
                File.AppendAllText("C:/Users/adelamare/Downloads/Thomas aka le stagiaire/HappyMeal_v3/HappyMeal_v3/Marks.txt", json);
            }

            else
            {

                bool IsExist = false;

                foreach (var e in dico)
                {
                    if (e.Key == food)
                    {
                        int finalmark = (e.Value + mark) / 2;
                        dico[e.Key] = finalmark;
                        IsExist = true;
                        break;
                    }
                }

                if (!IsExist)
                {
                    dico.Add(food, mark);
                }

                string json = JsonConvert.SerializeObject(dico, Formatting.Indented);

                File.Delete("C:/Users/adelamare/Downloads/Thomas aka le stagiaire/HappyMeal_v3/HappyMeal_v3/Marks.txt");
                File.AppendAllText("C:/Users/adelamare/Downloads/Thomas aka le stagiaire/HappyMeal_v3/HappyMeal_v3/Marks.txt", json);
            }
        }
    }
}
