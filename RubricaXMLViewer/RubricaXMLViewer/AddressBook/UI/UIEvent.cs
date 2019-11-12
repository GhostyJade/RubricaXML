using System;

namespace RubricaXMLViewer.AddressBook.UI
{
    public class UIEvent
    {

        public event Action Action;
        public event Func<bool> Condition;
        public bool Completed { get; private set; } = false;

        public UIEvent()
        {
        }

        public void MarkAsCompleted()
        {
            Completed = true;
        }

        public void PerformAction()
        {
            Action.Invoke();
        }
        public bool ActionConditions()
        {
            return Condition.Invoke();
        }
    }
}
