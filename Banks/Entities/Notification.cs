namespace Banks.Entities
{
    public class Notification
    {
        public Notification(string messageType, string previousValue, string newValue)
        {
            MessageType = messageType;
            PreviousValue = previousValue;
            NewValue = newValue;
        }

        public string MessageType { get; }
        public string PreviousValue { get; }
        public string NewValue { get; }

        public override string ToString()
        {
            return $"Changed: {MessageType}\nFrom: {PreviousValue};\nTo: {NewValue}";
        }
    }
}