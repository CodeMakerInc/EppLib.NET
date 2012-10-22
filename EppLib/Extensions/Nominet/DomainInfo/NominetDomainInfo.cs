using System;
using System.Xml;
using EppLib.Entities;

namespace EppLib.Extensions.Nominet.DomainInfo
{
	public class NominetDomainInfo : DomainBase<NominetDomainInfoResponse>
	{
		private readonly string domainName;

		public string Hosts { get; set; }

		public NominetDomainInfo(string domainName)
		{
			this.domainName = domainName;
		}

		protected override XmlElement BuildCommandElement(XmlDocument doc, XmlElement commandRootElement)
		{
			var domainInfo = BuildCommandElement(doc, "info", commandRootElement);

			var domainNameElement = AddXmlElement(doc, domainInfo, "domain:name", domainName, namespaceUri);

			if (!String.IsNullOrEmpty(Hosts))
			{
				domainNameElement.SetAttribute("hosts", Hosts);
			}

			return domainInfo;
		}

		public override NominetDomainInfoResponse FromBytes(byte[] bytes)
		{
			return new NominetDomainInfoResponse(bytes);
		}
	}
}