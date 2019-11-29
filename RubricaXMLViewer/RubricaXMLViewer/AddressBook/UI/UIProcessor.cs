using RubricaXMLViewer.AddressBook.Data;
using RubricaXMLViewer.AddressBook.Utils;
using RubricaXMLViewer.Windows;
using System.Collections.Generic;
using System.Windows;

namespace RubricaXMLViewer.AddressBook.UI
{
    public class UIProcessor
    {

        public static UIProcessor Instance { get; private set; } = new UIProcessor();

        private ConcurrentObservableCollection<UIEvent> UIEvents = new ConcurrentObservableCollection<UIEvent>();
        
        public void Init()
        {
            UIEvents.CollectionChanged += (sender, args) =>
            {
                for (int i = 0; i < UIEvents.Count; i++)
                {
                    if (UIEvents[i].IsCompleted())
                        UIEvents.RemoveAt(i);
                    else
                        UIEvents[i].PerformAction();
                }
                return;
            };
        }

        public void ParseAction(string action, params string[] args)
        {
            Dictionary<string, string> data = ParseData(args);
            UIEvent e = new UIEvent();
            switch (action)
            {
                case "NewAddressBook":
                    if (data.ContainsKey("result"))
                    {
                        if (data.TryGetValue("result", out string val))
                        {
                            if (val == "succeeded")
                            {
                                data.TryGetValue("name", out string name);
                                e.SetAction(() =>
                                  {
                                      MessageBox.Show($"Address book {name} successfully created.");
                                      e.MarkAsCompleted();
                                  });
                            }
                            else
                            {
                                e.SetAction(() =>
                                {
                                    MessageBox.Show("Failed to create new address book");
                                    e.MarkAsCompleted();
                                });
                            }
                        }
                    }
                    break;
                case "NewEntry":
                    if (data.ContainsKey("result"))
                    {

                        if (data.TryGetValue("result", out string value))
                        {
                            if (value == "succeeded")
                            {
                                data.TryGetValue("bn", out string bookName);
                                data.TryGetValue("name", out string name);
                                data.TryGetValue("surname", out string surname);
                                data.TryGetValue("phone", out string phone);
                                e.SetAction(() =>
                               {
                                   MainWindow.Instance.AddEntry(new AddressBookEntry()
                                   {
                                       Name = name,
                                       Surname = surname,
                                       PhoneNumber = phone
                                   });
                                   e.MarkAsCompleted();
                               });
                            }
                        }
                    }
                    break;
                case "BookList":
                    if (data.TryGetValue("length", out string booksLengthString))
                    {
                        List<string> bookNames = new List<string>();
                        if (int.TryParse(booksLengthString, out int length))
                        {
                            for (int i = 0; i < length; i++)
                            {
                                data.TryGetValue("name" + i, out string book);
                                bookNames.Add(book);
                            }
                        }
                        e.SetAction(() =>
                       {
                           new ChangeAddressBook(bookNames).Show();
                           e.MarkAsCompleted();
                       });
                    }
                    break;
                case "ContactList":
                    if (data.TryGetValue("length", out string lengthString))
                    {
                        List<AddressBookEntry> entries = new List<AddressBookEntry>();
                        if (int.TryParse(lengthString, out int length))
                        {
                            for (int i = 0; i < length; i++)
                            {
                                if (data.TryGetValue("index" + i, out string part))
                                {
                                    Dictionary<string, string> entryArgsDict = new Dictionary<string, string>();
                                    foreach (string s in part.Split('|'))
                                    {
                                        string[] entryArgs = s.Split(':');
                                        entryArgsDict.Add(entryArgs[0], entryArgs[1]);
                                    }
                                    entries.Add(new AddressBookEntry()
                                    {
                                        Name = entryArgsDict.GetData("name"),
                                        Surname = entryArgsDict.GetData("surname"),
                                        PhoneNumber = entryArgsDict.GetData("phone"),
                                    });
                                }
                            }
                        }
                        e.SetAction(() =>
                        {
                            MainWindow.Instance.SetData(entries);
                            e.MarkAsCompleted();
                        });
                    }
                    break;
            }
            UIEvents.Add(e);
        }

        private Dictionary<string, string> ParseData(string[] args)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach (string s in args)
            {
                string[] parts = s.Split('=');
                
                data.Add(parts[0], parts[1]);
            }

            return data;
        }
    }
}
