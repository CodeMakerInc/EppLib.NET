using System;
using System.Xml;
using EppLib.Entities;

namespace EppLib.Extensions.Nominet.ContactInfo
{
	public class NominetContactInfoResponse : ContactInfoResponse
	{
		private NominetContactExtension _nomContact = new NominetContactExtension();
		public virtual new NominetContactExtension Contact
		{
			get { return _nomContact; }
		}

		public NominetContactInfoResponse(byte[] bytes) : base(bytes) { }

		protected override void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
		{
			base.ProcessDataNode(doc, namespaces);

			_nomContact.Id = _contact.Id;
			_nomContact.Voice = _contact.Voice;
			_nomContact.Fax = _contact.Fax;
			_nomContact.Email = _contact.Email;
			_nomContact.PostalInfo = _contact.PostalInfo;
			_nomContact.Roid = _contact.Roid;
			_nomContact.Status = _contact.Status;
			_nomContact.ClId = _contact.ClId;
			_nomContact.CrId = _contact.CrId;
			_nomContact.CrDate = _contact.CrDate;
		}

		protected override void ProcessExtensionNode(XmlDocument doc, XmlNamespaceManager namespaces)
		{
			namespaces.AddNamespace("contact", "urn:ietf:params:xml:ns:contact-1.0");
			namespaces.AddNamespace("contact-nom-ext", "http://www.nominet.org.uk/epp/xml/contact-nom-ext-1.0");

			var children = doc.SelectSingleNode("/ns:epp/ns:response/ns:extension/contact-nom-ext:infData", namespaces);

			if (children != null)
			{
				var tradeNameNode = children.SelectSingleNode("contact-nom-ext:trad-name", namespaces);

				if (tradeNameNode != null)
				{
					_nomContact.TradeName = tradeNameNode.InnerText;
				}

				var coNoNode = children.SelectSingleNode("contact-nom-ext:co-no", namespaces);

				if (coNoNode != null)
				{
					_nomContact.CompanyNumber = coNoNode.InnerText;
				}

				var typeNode = children.SelectSingleNode("contact-nom-ext:type", namespaces);

				if (typeNode != null)
				{
					_nomContact.Type = (CoType)Enum.Parse(typeof(CoType), typeNode.InnerText);
				}

				var optOutNode = children.SelectSingleNode("contact-nom-ext:opt-out", namespaces);
				if (optOutNode != null)
				{
					_nomContact.OptOut = (YesNoFlag)Enum.Parse(typeof(YesNoFlag), optOutNode.InnerText);
				}
			}
		}
	}
}