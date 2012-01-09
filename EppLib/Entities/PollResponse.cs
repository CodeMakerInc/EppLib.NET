using System;
using System.Globalization;
using System.Xml;

namespace EppLib.Entities
{
    public class PollResponse : EppResponse
    {
        public string MsgId;
        public string DomainName;

        public PollResponse(byte[] bytes) : base(bytes)
        {
            
        }

        protected override void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            var messageNode = doc.SelectSingleNode("/ns:epp/ns:response/ns:msgQ", namespaces);

            if (messageNode != null)
            {
               
                if (messageNode.Attributes != null)
                {
                    Id = messageNode.Attributes["id"].Value;
                    Count = Convert.ToInt32(messageNode.Attributes["count"].Value,CultureInfo.InvariantCulture);
                }

                var qDateNode = messageNode.SelectSingleNode("ns:qDate", namespaces);

                if(qDateNode!=null)
                {
                    QDate = qDateNode.InnerText;
                }

                var msgNode = messageNode.SelectSingleNode("ns:msg", namespaces);

                if (msgNode != null)
                {
                    Body = msgNode.InnerText;

                    if (msgNode.Attributes != null)
                    {
                        Language = msgNode.Attributes["lang"].Value;
                    }
                }
                
            }

            var resData = doc.SelectSingleNode("/ns:epp/ns:response/ns:resData", namespaces);

            namespaces.AddNamespace("poll","urn:ietf:params:xml:ns:poll-1.0");

            if (resData != null)
            {
                var msgIDNode = resData.SelectSingleNode("poll:msgID", namespaces);

                if (msgIDNode != null)
                {
                    MsgId = msgIDNode.InnerText;
                }

                var domainNameNode = resData.SelectSingleNode("poll:domainName", namespaces);

                if (domainNameNode != null)
                {
                    DomainName = domainNameNode.InnerText;
                }
            }
        }

        public string Id { get; set; }
        public int Count { get; set; }
        public string QDate { get; set; }
        public string Body { get; set; }
        public string Language { get; set; }
    }
}
