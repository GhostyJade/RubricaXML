namespace RubricaXMLViewer.AddressBook.Data
{
    public class Property
    {
        public string Key { get; private set; }
        public string Value { get; private set; }

        public Property(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public Property(string data)
        {
            string[] parts = data.Split('=');
            Key = parts[0];
            Value = parts[1];
        }
    }
}
