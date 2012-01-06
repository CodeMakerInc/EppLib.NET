namespace EppLib.Entities
{
    public class Telephone
    {
        public string Value;
        public string Extension;

        public Telephone(string value, string extension)
        {
            Value = value;
            Extension = extension;
        }

        public Telephone()
        {
        }
    }
}