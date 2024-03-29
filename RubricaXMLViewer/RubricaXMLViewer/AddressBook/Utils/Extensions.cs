﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public static bool TryGetText(this TextBox tx, out string textContent)
        {
            string txt = tx.Text;
            if (string.IsNullOrEmpty(txt) || string.IsNullOrWhiteSpace(txt))
            {
                textContent = null;
                return false;
            }
            textContent = txt;
            return true;
        }

        public static void AddRange<T>(this ObservableCollection<T> src, IList<T> list)
        {
            foreach (T v in list)
            {
                src.Add(v);
            }
        }
    }
}
