namespace EasyEPP.Entities
{
    public class DomainStatus
    {
        public string Value;
        public string Type;
        public string Lang { get; set; }

        public DomainStatus(string value, string type)
        {
            Value = value;
            Type = type;
        }
    }
}