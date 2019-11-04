using System.Collections.Generic;
using System.IO;

namespace RubricaXMLViewer.AddressBook.Utils
{
    public class I18n
    {
        private Dictionary<string, string> translations = new Dictionary<string, string>();

        public I18n(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            foreach (string l in lines)
            {
                if (!(string.IsNullOrWhiteSpace(l) || string.IsNullOrEmpty(l)))
                {
                    string[] parts = l.Split('=');
                    translations.Add(parts[0], parts[1]);
                }
            }
        }

        public string GetTranslationString(string key)
        {
            return translations.TryGetValue(key, out string val) ? val : key;
        }
    }
}
