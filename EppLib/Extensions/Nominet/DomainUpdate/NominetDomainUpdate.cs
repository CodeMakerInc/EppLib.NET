using System.Collections.Generic;
using System.Xml;

namespace EppLib.Extensions.Nominet.DomainUpdate
{
	public class NominetDomainUpdate : Entities.DomainUpdate
    {
		public string FirstBill { get; set; }
		public string RecurBill { get; set; }
		public string AutoBill { get; set; }
		public string NextBill { get; set; }
		public string Reseller { get; set; }
		public string NextPeriod { get; set; }
		public string AutoPeriod { get; set; }
		public string RenewNotRequired { get; set; }
		public List<string> Notes { get; set; }

		public NominetDomainUpdate(string domainName) : base(domainName) { }

		public override XmlDocument ToXml()
		{
			var updateExt = new NominetDomainUpdateExtension
			                	{
			                		AutoBill = AutoBill,
			                		FirstBill = FirstBill,
			                		NextBill = NextBill,
			                		RecurBill = RecurBill,
			                		Reseller = Reseller,
			                		Notes = Notes,
			                		AutoPeriod = AutoPeriod,
			                		NextPeriod = NextPeriod,
			                		RenewNotRequired = RenewNotRequired
			                	};

			Extensions.Clear();
			Extensions.Add(updateExt);

			return base.ToXml();
        }
    }
}