namespace EppLib.Entities
{
    public class DomainPeriod
    {
        public int Value { get; set; }

        public string Unit { get; set; }

        public DomainPeriod(int value, string unit)
        {
            Value = value;
            Unit = unit;
        }
    }
}