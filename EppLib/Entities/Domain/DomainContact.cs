using System;

namespace EppLib.Entities
{
    public class DomainContact
    {
        public readonly string Id;

        public readonly string Type;

        public DomainContact(string id, string type)
        {
            Id = id;
            Type = type;
        }
    }
}