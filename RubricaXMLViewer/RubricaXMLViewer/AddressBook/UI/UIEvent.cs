namespace RubricaXMLViewer.AddressBook.UI
{
    public class UIEvent
    {
        public event System.Action Action;
        public event System.Func<bool> Condition;
        private volatile bool Completed = false;

        public UIEvent() { }

        public void MarkAsCompleted()
        {
            Completed = true;
        }

        public bool IsCompleted()
        {
            return Completed;
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
