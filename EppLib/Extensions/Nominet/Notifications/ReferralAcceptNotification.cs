using System.Xml;
using EppLib.Entities;

namespace EppLib.Extensions.Nominet.Notifications
{
	public class ReferralAcceptNotification : PollResponse
	{
		public DomainCreateResult DomainCreateResult { get; set; }

		public ReferralAcceptNotification(string xml) : base(xml) { }
		public ReferralAcceptNotification(byte[] bytes) : base(bytes) { }

		protected override void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
		{
			base.ProcessDataNode(doc, namespaces);

			namespaces.AddNamespace("domain", "urn:ietf:params:xml:ns:domain-1.0");
			var domainCreateDataNode = doc.SelectSingleNode("/ns:epp/ns:response/ns:resData/domain:creData", namespaces);

			if (domainCreateDataNode != null)
			{
				var domainRes = new DomainCreateResponse(domainCreateDataNode.OuterXml);
				DomainCreateResult = domainRes.DomainCreateResult;
			}
		}
	}
}
