using System.Collections.Generic;
using System.Windows.Controls;

namespace RubricaXMLViewer.AddressBook.Utils
{
    public static class Extensions
    {
        public static string GetData(this Dictionary<string, string> d, string key)
        {
            return d.TryGetValue(key, out string value) ? value : "";
        }

        public static string TryGetText(this TextBox tx)
        {
            return (string.IsNullOrEmpty(tx.Text) || string.IsNullOrWhiteSpace(tx.Text)) ? "" : tx.Text;
        }
    }
}
