namespace EppLib.Entities
{
    public abstract class HostBase<T> : EppCommand<T> where T : EppResponse
    {
        protected HostBase() : base("host", "urn:ietf:params:xml:ns:host-1.0")
        {
        }
    }
}
