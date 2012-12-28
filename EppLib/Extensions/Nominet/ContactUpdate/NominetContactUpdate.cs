using System.Xml;

namespace EppLib.Extensions.Nominet.ContactUpdate
{
	public class NominetContactUpdate : Entities.ContactUpdate
    {
		public string TradeName { get; set; }
		public CoType? Type { get; set; }
		public string CompanyNumber { get; set; }
		public YesNoFlag? OptOut { get; set; }

		public NominetContactUpdate(string contactId) : base(contactId) { }

		public override XmlDocument ToXml()
		{
			var updateExt = new NominetContactUpdateExtension
			                	{
			                		TradeName = TradeName,
			                		Type = Type,
			                		CompanyNumber = CompanyNumber,
			                		OptOut = OptOut
			                	};

			Extensions.Clear();
			Extensions.Add(updateExt);

			return base.ToXml();
        }
    }
}