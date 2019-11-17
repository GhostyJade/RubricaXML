using System;
using System.Windows;

namespace RubricaXMLViewer.AddressBook.UI
{
    public class UIEvent
    {
        private Action Action;
        //public event Func<bool> Condition;
        private bool Completed = false;

        public UIEvent() { }

        public void MarkAsCompleted()
        {
            Completed = true;
        }

        public void SetAction(Action a)
        {
            Action = a;
        }

        public bool IsCompleted()
        {
            return Completed;
        }

        public void PerformAction()
        {
            if (!Completed)
            {
                Application.Current.Dispatcher.BeginInvoke(Action) ;
                return;
            }
        }
        /*public bool ActionConditions()
        {
            return Condition.Invoke();
        }*/
    }
}
