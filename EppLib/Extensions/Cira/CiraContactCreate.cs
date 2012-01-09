using System.Xml;
using EppLib.Entities;

namespace EppLib.Extensions.Cira
{
    public class CiraContactCreate : ContactCreate
    {
        public CiraContactCreate(Contact contact) : base(contact)
        {
        }

        public override XmlDocument ToXml()
        {
            var ciraExtension = new CiraCreateExtension
            {
                Language = Language,
                OriginatingIpAddress = OriginatingIpAddress,
                CprCategory = CprCategory,
                AgreementVersion = AgreementVersion,
                AggreementValue = AggreementValue,
                CreatedByResellerId = CreatedByResellerId
            };

            Extensions.Add(ciraExtension);

            return base.ToXml();
        }

        public string CreatedByResellerId { get; set; }

        public string AggreementValue { get; set; }

        public string AgreementVersion { get; set; }

        public string CprCategory { get; set; }

        public string OriginatingIpAddress { get; set; }

        public string Language { get; set; }
    }
}
