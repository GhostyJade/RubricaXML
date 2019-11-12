using System;

namespace RubricaXMLViewer.AddressBook.UI
{
    public class UIEvent
    {

        private event Action action;
        private event Func<bool> condition;

        public UIEvent(Action action, Func<bool> condition)
        {
            this.action = action;
            this.condition = condition;
        }

        public void PerformAction()
        {
            action.Invoke();
        }
        public bool ActionConditions()
        {
            return condition.Invoke();
        }
    }
}
