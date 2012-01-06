using System;
using System.Collections.Generic;
using System.Xml;
using EppLib.Entities;

namespace EppLib.Extensions.Cira
{
    public class CiraTransferExtension : EppExtension
    {
        private IList<string> techContactIds = new List<string>();
        
        public IList<string> TechContactIds
        {
            get { return techContactIds; }
            set { techContactIds = value; }
        }

        public string AdminContactId { get; set; }

        public string RegistrantContactId { get; set; }

        public override XmlNode ToXml(XmlDocument doc)
        {
            var root = CreateElement(doc, "cira:ciraTransfer");

            var ciraChg = AddXmlElement(doc, root, "cira:ciraChg", "");


            if (!String.IsNullOrEmpty(RegistrantContactId))
            {
                AddXmlElement(doc, ciraChg, "cira:registrant", RegistrantContactId);
            }

            if(!String.IsNullOrEmpty(AdminContactId))
            {
                var adminContact = AddXmlElement(doc, ciraChg, "cira:contact", AdminContactId);
                adminContact.SetAttribute("type", "admin");
            }

            if (TechContactIds != null)
            {
                foreach (var techContactId in TechContactIds)
                {
                    if (techContactId != null)
                    {
                        var techContact = AddXmlElement(doc, ciraChg, "cira:contact", techContactId);
                        techContact.SetAttribute("type", "tech");
                    }
                }
            }

            return root;
        }
    }
}