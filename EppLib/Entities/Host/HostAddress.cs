namespace EppLib.Entities
{
    public class HostAddress
    {
        public HostAddress(string ipAddress, string ipVersion)
        {
            IPAddress = ipAddress;
            IPVersion = ipVersion;
        }

        public HostAddress()
        {
        }

        public string IPAddress { get; set; }
        public string IPVersion { get; set; }
    }
}