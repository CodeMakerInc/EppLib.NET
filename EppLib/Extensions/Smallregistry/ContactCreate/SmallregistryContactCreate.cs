using EppLib.Entities;
using System;
using System.Xml;

namespace EppLib.Extensions.Smallregistry.ContactCreate
{
    public class SmallregistryContactCreate : Entities.ContactCreate
    {
        public string CompanySerial { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public SmallregistryContactCreate(Contact contact) : base(contact)
        {
            
        }

        public override XmlDocument ToXml()
        {
            if (CompanySerial != null)
            {
                var createExt = new SmallregistryContactPMCreateExtension
                {
                    CompanySerial = CompanySerial
                };

                Extensions.Clear();
                Extensions.Add(createExt);
            }
            else
            {
                var createExt = new SmallregistryContactPPCreateExtension
                {
                    BirthDate = BirthDate,
                    BirthPlace = BirthPlace
                };

                Extensions.Clear();
                Extensions.Add(createExt);
            }
            return base.ToXml();
        }
    }
}
