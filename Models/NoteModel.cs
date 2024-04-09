using System.IO;

namespace CrayNotes.WPFUI.Models
{
    public class NoteModel(string path)
    {
        private string notePath = path;

        public string NotePath
        {
            get { return notePath; }
            set { notePath = value; }
        }

        public string Name
        {
            get { return Path.GetFileNameWithoutExtension(notePath); }
            private set { notePath = value; }
        }

        // stores contents in program memory once loaded
        private string? contents;

        public string? Contents
        {
            get { return contents; }
            set { contents = value; }
        }
        
        // loads text from the note to the field and returns text to caller
        public string LoadContents()
        {
            System.Diagnostics.Trace.WriteLine($"Loading Contents by Note Path: {NotePath}");
            string text = "";
            using (StreamReader reader = new StreamReader(NotePath))
            {
                text += reader.ReadToEnd();
                reader.Close();
            }
            System.Diagnostics.Trace.WriteLine($"contents: {text}");
            Contents = text;
            return text;
        }

        public void SaveContents()
        {
            using (StreamWriter writer = new StreamWriter(NotePath, false))
            {
                writer.WriteLineAsync(Contents);
                writer.Close();
            }
        }
    }
}
