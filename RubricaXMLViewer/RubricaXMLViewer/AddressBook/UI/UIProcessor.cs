using RubricaXMLViewer.AddressBook.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows;

namespace RubricaXMLViewer.AddressBook.UI
{
    public class UIProcessor
    {

        public static UIProcessor Instance { get; private set; } = new UIProcessor();

        private SynchronizedCollection<UIEvent> UIEvents = new SynchronizedCollection<UIEvent>(); //TODO concurrent.
        private MainWindow mainWindow;

        public void Init(MainWindow window)
        {
            mainWindow = window;
        }

        public void Update()
        {
            foreach (UIEvent e in UIEvents)
                if (e.ActionConditions())
                    e.PerformAction();

            for (int i = UIEvents.Count; i > 0; i--)
            {
                UIEvent e = UIEvents[i];
                if (e.Completed)
                {
                    UIEvents.Remove(e);
                }
            }
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
                                      Application.Current.Dispatcher.Invoke((Action)delegate
                                      {
                                          Instances.Books.Add(name);
                                      });
                                      e.MarkAsCompleted();
                                  };
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
                    UIEvents.Add(e);
                    break;
            }
            Console.WriteLine(UIEvents.Count.ToString());
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
