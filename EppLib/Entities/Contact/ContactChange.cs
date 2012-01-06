using System;

namespace EppLib.Entities
{
    public class ContactChange
    {
        public PostalInfo PostalInfo;
        public Telephone Voice;
        public Telephone Fax;
        public string Email;

        public ContactChange(Contact contact)
        {
            this.Email = contact.Email;
            this.Voice = contact.Voice;
            this.Fax = contact.Fax;

            this.PostalInfo = contact.PostalInfo;
        }

        public ContactChange()
        {
        }
    }
}