using System.Xml;
using EppLib.Entities;

namespace EppLib.Extensions.Cira
{
    public class CiraContactUpdateExtension : EppExtension
    {
        public string Language;
        public string CprCategory;
        public string WhoisDisplaySetting;

        public override XmlNode ToXml(XmlDocument doc)
        {
            var root = CreateElement(doc, "cira:ciraUpdate");

            var ciraChg = AddXmlElement(doc, root, "cira:ciraChg", "");
            
            if (Language != null)
            {
                AddXmlElement(doc, ciraChg, "cira:language", Language);
            }

            if (CprCategory != null)
            {
                AddXmlElement(doc, ciraChg, "cira:cprCategory", CprCategory);
            }

            if (WhoisDisplaySetting != null)
            {
                AddXmlElement(doc, ciraChg, "cira:whoisDisplaySetting", WhoisDisplaySetting);
            }

            return root;
        }
    }
}