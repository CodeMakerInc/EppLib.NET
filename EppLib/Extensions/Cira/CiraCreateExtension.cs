using System.Xml;
using EppLib.Entities;

namespace EppLib.Extensions.Cira
{
    public class CiraCreateExtension : EppExtension
    {
        public string Language;
        public string OriginatingIpAddress;
        public string CprCategory;
        public string AgreementVersion;
        public string AggreementValue;
        public string CreatedByResellerId;
        
        public override XmlNode ToXml(XmlDocument doc)
        {
            var root = CreateElement(doc,"cira:ciraCreate");

            if (Language != null)
            {
                AddXmlElement(doc, root, "cira:language", Language);
            }
           
            if (CprCategory != null)
            {
                AddXmlElement(doc, root, "cira:cprCategory", CprCategory);

                //The CIRA Originating IP Address is valid only for Registrants with an assigned CPR category
                if (OriginatingIpAddress != null)
                {
                    AddXmlElement(doc, root, "cira:originatingIpAddress", OriginatingIpAddress);
                }

                //The CIRA Created by Reseller ID is valid only for Registrants with an assigned CPR category
                if (CreatedByResellerId != null)
                {
                    AddXmlElement(doc, root, "cira:createdByResellerId", CreatedByResellerId);
                }
            }

            if (AgreementVersion != null)
            {
                AddXmlElement(doc, root, "cira:ciraAgreementVersion", AgreementVersion);
            }

            if (AggreementValue != null)
            {
                AddXmlElement(doc, root, "cira:agreementValue", AggreementValue);
            } 
            
            return root;
        }

    }
}
