using RubricaXMLViewer.AddressBook.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace RubricaXMLViewer.AddressBook.UI
{
    public class UIProcessor
    {

        public static UIProcessor Instance { get; private set; } = new UIProcessor();

        private static volatile ObservableCollection<UIEvent> UIEvents = new ObservableCollection<UIEvent>();

        public void Init()
        {
            UIEvents.CollectionChanged += (sender, args) =>
            {
                Update();
            };
            UIEvent e = new UIEvent();
            e.Action += () => { e.MarkAsCompleted(); };
            e.Condition += () => true;
            UIEvents.Add(e);
        }

        public void Update()
        {
            /*List<UIEvent> eventsToMaintain = new List<UIEvent>();
            bool success = UIEvents.TryTake(out UIEvent result);
            while (success)
            {
                if (!result.IsCompleted())
                    eventsToMaintain.Add(result);
                success = UIEvents.TryTake(out result);
            }
            eventsToMaintain.ForEach(e => UIEvents.Add(e));*/

            foreach (UIEvent e in UIEvents)
            {
                if (!e.IsCompleted())
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        e.PerformAction();
                    });
                }
            }
        }

        public void AddAction(UIEvent e)
        {
            UIEvents.Add(e);
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
                                e.Action += () =>
                                  {
                                      Application.Current.Dispatcher.Invoke(
                                          delegate
                                          {
                                              Instances.Books.Add(name);
                                          });
                                      e.MarkAsCompleted();
                                  };
                                e.Condition += () => true;
                            }
                            else
                            {
                                e.Action += () =>
                                {
                                    MessageBox.Show("Failed to create new address book");
                                    e.MarkAsCompleted();
                                };
                            }
                            e.Condition += () => true;
                        }
                    }
                    break;
                case "NewEntry":
                    if (data.ContainsKey("result"))
                    {

                        if (data.TryGetValue("result", out string value))
                        {
                            System.Console.WriteLine("Value=" + value);
                            if (value == "succeeded")
                            {
                                data.TryGetValue("bn", out string bookName);
                                data.TryGetValue("name", out string name);
                                data.TryGetValue("surname", out string surname);
                                data.TryGetValue("phone", out string phone);
                                System.Console.WriteLine("BookName={0}, Name={1}, Surname={2}, Phone={3}", bookName, name, surname, phone);
                                e.Action += () =>
                                {
                                    Application.Current.Dispatcher.Invoke(delegate
                                    {
                                        Instances.Entries.Add(new Data.AddressBookEntry()
                                        {
                                            Name = name,
                                            Surname = surname,
                                            PhoneNumber = phone
                                        });
                                        System.Console.WriteLine("Items: {0}", Instances.Entries.Count);
                                    });
                                    e.MarkAsCompleted();
                                };
                                e.Condition += () => true;
                            }
                        }
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

        public void Exit()
        {
            // UIEvents.Dispose();
        }
    }
}
