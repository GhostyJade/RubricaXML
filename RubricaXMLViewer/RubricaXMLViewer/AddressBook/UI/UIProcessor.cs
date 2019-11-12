using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubricaXMLViewer.AddressBook.UI
{
    public class UIProcessor
    {
        private List<UIEvent> UIEvents = new List<UIEvent>();
        private MainWindow mainWindow;

        public UIProcessor(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void Update()
        {
            UIEvents.ForEach(e =>
            {
                if (e.ActionConditions()) e.PerformAction();
            });
        }

        public void ParseAction(string action, params string[] args)
        {
            switch (action)
            {
                case "":
                    UIEvents.Add(new UIEvent(
                        () =>
                        {

                        }, () =>
                        {
                            return false;
                        }
                        ));
                    break;
            }
        }
    }
}
