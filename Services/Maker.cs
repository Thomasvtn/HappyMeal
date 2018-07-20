using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using HappyMeal_v3.Models;

namespace HappyMeal_v3.Services
{
    class Maker
    {
        public Choose Choose;

        private string tempFile;

        public readonly string Path = ConfigurationService.GetValue("path");
        public readonly string PathTemp = ConfigurationService.GetValue("pathTemp");
        public readonly string PathMenus = ConfigurationService.GetValue("pathMenus");

        public readonly string PathDocx;
        public readonly string PathXml;
        public readonly string PathFolder;
        public readonly string PathZip;

        public string Text { get; private set; } = string.Empty;

        public Maker(string file, bool mail = false)
        {
            tempFile = Guid.NewGuid().ToString();           

            PathDocx = mail ? $@"{PathTemp}\{file}.docx" : $@"{PathMenus}\{file}.docx";        

            PathXml = $@"{PathTemp}\{tempFile}\word\document.xml";
            PathFolder = $@"{PathTemp}\{tempFile}";
            PathZip = $@"{PathTemp}\{tempFile}.zip";

            if (!Directory.Exists($@"{Path}\Menus"))
                Directory.CreateDirectory($@"{Path}\Menus");
            if (!Directory.Exists($@"{Path}\Temp"))
                Directory.CreateDirectory($@"{Path}\Temp");
        }

        public void Init(bool parsing = false)
        {
            Text = ConvertStreamToString();
            Choose = new Choose(Text);

            if (parsing)
                DeleteFolder();
        }

        // Taille maximale du message : 140 caractères. 
        // Message = Nom Prénom Société.
        public string MakeDoc(List<Choice> cells, string message)
        {
            var extras = new Dictionary<Extra, int>();
            if (cells.Any(c => c.Id == 0)) extras.Add(Extra.Bread, cells.Where(c => c.Id == 0).Select(c => c.Quantity).First());
            if (cells.Any(c => c.Id == 1)) extras.Add(Extra.Cultery, cells.Where(c => c.Id == 1).Select(c => c.Quantity).First());

            var text = Choose.InsertChooses(Text, cells);
            text = Choose.InsertMessage(text, message, extras);
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            File.WriteAllText(PathXml, text);

            return ConvertFolderToDocx(message);
        }

        // A utiliser pour sauvegarder la pièce jointe
        // du mail une fois sa date vérifié.
        public void Save()
        {
            ConvertFolderToDocx();
        }

        private string ConvertStreamToString()
        {
            ConvertDocxToFolder();

            var text = string.Empty;
            using (var sr = new StreamReader(PathXml))
            {
                var line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                    text += line;
                sr.Close();
            }
            return text;
        }

        private void ConvertDocxToFolder()
        {
            if (!Directory.Exists(PathFolder))
            {
                File.Copy(PathDocx, PathZip);
                ZipFile.ExtractToDirectory(PathZip, PathFolder);
                if (File.Exists(PathZip))
                    File.Delete(PathZip);
            }
        }

        // Si le message est null, cela signifie qu'il s'agit
        // de la pièce jointe qui est sauvegardé.
        private string ConvertFolderToDocx(string message = null)
        {
            string file;
            if (string.IsNullOrEmpty(message))
            {
                var time = DateTime.Now;
                file = $"{PathMenus}/Menu_{time.Day:00}{time.Month:00}{time.Year}.docx";
            }
            else
            {
                message = message.Replace(" - TRSb", string.Empty).Replace(" ", string.Empty);
                var invalids = System.IO.Path.GetInvalidFileNameChars();
                foreach (var invalid in invalids)
                    message = message.Replace(invalid.ToString(), string.Empty);
                file = $"{PathTemp}/Menu_{message}.docx";
            }
            
            ZipFile.CreateFromDirectory(PathFolder, PathZip);
            DeleteFolder();
            if (string.IsNullOrEmpty(message))
            {
                if (File.Exists(PathDocx))
                    File.Delete(PathDocx);
            }

            if (File.Exists(file))
                File.Delete(file);
            File.Move(PathZip, System.IO.Path.ChangeExtension(file.Substring(0, file.Length - 5) + ".zip" , ".docx"));
            return file;
        }

        public void DeleteFile()
        {
            DeleteFolder();
            File.Delete(PathDocx);
        }

        private void DeleteFolder()
        {
            if (Directory.Exists(PathFolder))
            {
                var directory = new DirectoryInfo(PathFolder);
                foreach (var file in directory.GetFiles()) file.Delete();
                Directory.Delete(PathFolder, true);
            }
        }
    }
}