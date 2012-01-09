namespace EasyEPP.Entities
{
    public class HostStatus
    {
        public HostStatus(string value, string type)
        {
            Value = value;
            Type = type;
        }

        public HostStatus()
        {
        }

        public string Value { get; set; }

        public string Type { get; set; }

        public string Lang { get; set; }
    }
}