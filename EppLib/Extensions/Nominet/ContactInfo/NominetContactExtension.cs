using EppLib.Entities;

namespace EppLib.Extensions.Nominet.ContactInfo
{
	public class NominetContactExtension : Contact
	{
		public string TradeName { get; set; }
		public CoType? Type { get; set; }
		public string CompanyNumber { get; set; }
		public YesNoFlag? OptOut { get; set; }
	}
}