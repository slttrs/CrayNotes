using CrayNotes.WPFUI.Models;
using System.IO;

namespace CrayNotes.WPFUI.DataAccess
{
    public class DataAccess
    {
        /* to do:
        retrieve the list of notes in the folder
        open file, send contents to ShellViewModel
        create file
        delete file
        save file
        */

        private string dataPath;

        public string DataPath
        {
            get { return dataPath; }
            set { dataPath = value; }
        }

        public DataAccess() // get configured data path
        {
            System.Diagnostics.Trace.WriteLine("Constructing DataAccess");
            // set default data path
            DataPath = Path.Combine(Directory.GetCurrentDirectory(), "Notes");
            string configPath = Path.Combine(Directory.GetCurrentDirectory(), "config.txt");

            // open or create config file and get data path
            if (File.Exists(configPath))
            {
                using (var reader = new StreamReader(configPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        switch (line[..line.IndexOf('=')])
                        {
                            case "DataPath":
                                DataPath = line[(line.IndexOf('=') + 1)..];
                                System.Diagnostics.Trace.WriteLine(DataPath);
                                break;
                        }
                    }
                    reader.Close();
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(configPath))
                {
                    sw.WriteLine($"DataPath={DataPath}");
                    sw.Close();
                }
            }

            Directory.CreateDirectory(DataPath); // if specified path doesn't exist, create it
        }

        public List<NoteModel> GetNotesList()
        {
            System.Diagnostics.Trace.WriteLine("GetNotesList");
            List<NoteModel> notesList = new List<NoteModel>();

            // read file list from the folder and filter .txt files
            foreach (string file in Directory.EnumerateFiles(DataPath, "*.*", SearchOption.TopDirectoryOnly))
            {
                if (file.EndsWith(".txt"))
                {
                    notesList.Add(new NoteModel(file));
                }
            }

            return notesList;
        }

        public NoteModel CreateNote()
        {
            System.Diagnostics.Trace.WriteLine("Create Note DataAccess");
            string notePath = GetUniqueFileName(Path.Combine(DataPath, "Untitled.txt"));
            File.Create(notePath).Close();
            return new NoteModel(notePath);
        }

        public static string GetUniqueFileName(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return filePath;
            }

            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);

            int number = 0;
            string newFilePath;
            do {
                number++;
                newFilePath = Path.Combine(directory, $"{fileName}_{number}{extension}");
            } while (File.Exists(newFilePath));

            return newFilePath;
        }
    }
}
