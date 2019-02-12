using EppLib.Entities;
using System.Xml;

namespace EppLib.Extensions.Idn
{
    public class IdnExtension : EppExtension
    {
        private string _ns = "urn:ietf:params:xml:ns:idn-1.0";

        private string table;
        private string uname;

        public IdnExtension(string table, string uname)
        {
            this.table = table;
            this.uname = uname;
        }

        protected override string Namespace
        {
            get { return _ns; }
            set { _ns = value; }
        }

        public override XmlNode ToXml(XmlDocument doc)
        {
            var root = CreateElement(doc, "idn:data");

            AddXmlElement(doc, root, "idn:table", table);
            AddXmlElement(doc, root, "idn:uname", uname);

            return root;
        }
    }
}
