using System.Collections.Generic;
using System.Xml;

namespace EppLib.Extensions.Nominet.DomainCreate
{
	public class NominetDomainCreate : Entities.DomainCreate
    {
		public string FirstBill { get; set; }
		public string RecurBill { get; set; }
		public string AutoBill { get; set; }
		public string NextBill { get; set; }
		public string Reseller { get; set; }
		public string NextPeriod { get; set; }
		public string AutoPeriod { get; set; }
		public List<string> Notes { get; set; }

		public NominetDomainCreate(string domainName, string registrantContactId)
			: base(domainName, registrantContactId) { }

		public override XmlDocument ToXml()
		{
			var ciraExtension = new NominetDomainCreateExtension
			                    	{
										AutoBill = AutoBill,
										FirstBill = FirstBill,
										NextBill = NextBill,
										RecurBill = RecurBill,
										Reseller = Reseller,
										Notes = Notes,
										AutoPeriod = AutoPeriod,
										NextPeriod = NextPeriod
			                    	};

			Extensions.Clear();
			Extensions.Add(ciraExtension);

			return base.ToXml();
        }
    }
}