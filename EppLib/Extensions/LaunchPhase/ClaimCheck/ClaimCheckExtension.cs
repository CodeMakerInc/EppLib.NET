using EppLib.Entities;
using System.Xml;

namespace EppLib.Extensions.LaunchPhase.ClaimCheck
{
    public class ClaimCheckExtension : EppExtension
    {
        protected override string Namespace { get; set; }

        public override XmlNode ToXml(XmlDocument doc)
        {
            var root = doc.CreateElement("launch:check", "urn:ietf:params:xml:ns:launch-1.0"); 
            root.SetAttribute("type", "claims");

            var xsd = doc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xsd.Value = "urn:ietf:params:xml:ns:launch-1.0 launch-1.0.xsd";
            root.Attributes.Append(xsd);

            root.SetAttribute("xmlns:launch", "urn:ietf:params:xml:ns:launch-1.0");

            root.InnerXml = "<launch:phase>claims</launch:phase>";

            return root;
        }
    }
}
