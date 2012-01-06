using System.Collections.Generic;

namespace EppLib.Entities
{
    public class EppHostUpdateAddRemove
    {
        public IList<HostAddress> Adresses { get; set; }
        public IList<Status> Status;
    }
}