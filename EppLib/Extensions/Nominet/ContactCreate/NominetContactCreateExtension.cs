using System.Xml;

namespace EppLib.Extensions.Nominet.ContactCreate
{
	public class NominetContactCreateExtension : NominetContactExtensionBase
	{
		public string TradeName { get; set; }
		public CoType? Type { get; set; }
		public string CompanyNumber { get; set; }
		public YesNoFlag? OptOut { get; set; }

		public override XmlNode ToXml(XmlDocument doc)
		{
			var root = CreateElement(doc, "contact-nom-ext:create");

			if (TradeName != null)
			{
				AddXmlElement(doc, root, "contact-nom-ext:trad-name", TradeName);
			}

			if (Type != null)
			{
				AddXmlElement(doc, root, "contact-nom-ext:type", Type.Value.ToString());
			}

			if (CompanyNumber != null)
			{
				AddXmlElement(doc, root, "contact-nom-ext:co-no", CompanyNumber);
			}

			if (OptOut != null)
			{
				AddXmlElement(doc, root, "contact-nom-ext:opt-out", OptOut.Value.ToString());
			}

			return root;
		}
	}
}