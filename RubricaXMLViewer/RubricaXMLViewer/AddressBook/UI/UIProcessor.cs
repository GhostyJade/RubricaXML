using RubricaXMLViewer.AddressBook.Utils;
using System;
using System.Collections.Generic;
using System.Windows;

namespace RubricaXMLViewer.AddressBook.UI
{
    public class UIProcessor
    {

        public static UIProcessor Instance { get; private set; } = new UIProcessor();

        private List<UIEvent> UIEvents = new List<UIEvent>();
        private MainWindow mainWindow;

        public void Init(MainWindow window)
        {
            mainWindow = window;
        }

        public void Update()
        {
            UIEvents.ForEach(e =>
            {
                if (e.ActionConditions()) e.PerformAction();
            });
            UIEvents.RemoveAll(item => item.Completed);
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
                                      Application.Current.Dispatcher.Invoke((Action)delegate { 
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
