namespace EppLib.Entities
{
    public class Status
    {
        public Status(string value, string type)
        {
            Value = value;
            Type = type;
        }

        
        public string Value { get; set; }

        public string Type { get; set; }

        public string Lang { get; set; }
    }
}