using System.Collections.Generic;

namespace EppLib.Entities
{
    public class EppDomainUpdateAddRemove
    {
        public IList<string> NameServers = new List<string>();
        public IList<DomainContact> DomainContacts = new List<DomainContact>();
        public IList<Status> Status = new List<Status>();
    }
}