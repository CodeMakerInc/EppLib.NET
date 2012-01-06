using System.IO;
using System.Xml;

namespace EppLib.Entities
{
    public class EppResponse
    {
        public EppResponse(byte[] bytes)
        {
            FromXmlString(bytes);
        }

        private void FromXmlString(byte[] bytes)
        {
            var doc = new XmlDocument();
            
            var namespaces = new XmlNamespaceManager(doc.NameTable);

            namespaces.AddNamespace("ns", "urn:ietf:params:xml:ns:epp-1.0");

            doc.Load(new MemoryStream(bytes));

            var resultNode = doc.SelectSingleNode("/ns:epp/ns:response/ns:result",namespaces);

            if (resultNode != null)
            {
                ProcessResultNode(doc, namespaces, resultNode);
            }

            ProcessDataNode(doc, namespaces);
           

            var extension = doc.SelectSingleNode("/ns:epp/ns:response/ns:extension", namespaces);

            if (extension != null)
            {
                ProcessExtensionNode(doc, namespaces);
            }
            
        }

        protected virtual void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            //default implementation does nothing
        }

        protected virtual void ProcessExtensionNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            //default implementation does nothing
        }

        private void ProcessResultNode(XmlDocument doc, XmlNamespaceManager namespaces, XmlNode resultNode)
        {
            if (resultNode.Attributes != null) Code = resultNode.Attributes["code"].Value;

            var msgNode = doc.SelectSingleNode("/ns:epp/ns:response/ns:result/ns:msg", namespaces);

            if (msgNode != null) {Message = msgNode.InnerText;}

            var extValueNode = doc.SelectSingleNode("/ns:epp/ns:response/ns:result/ns:extValue", namespaces);

            if (extValueNode != null)
            {
                var valueNode = extValueNode.SelectSingleNode("ns:value", namespaces);
                if (valueNode != null) ExtValue = valueNode.InnerText;

                var reasonNode = extValueNode.SelectSingleNode("ns:reason", namespaces);
                if (reasonNode != null) Reason = reasonNode.InnerText;
            }
        }

        public string Reason { get; private set; }
        public string ExtValue { get; private set; }
        public string Message { get; private set; }
        public string Code { get; private set; }
    }

}
