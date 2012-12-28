using System.Collections.Generic;
using EppLib.Entities;

namespace EppLib.Extensions.Nominet.DomainInfo
{
	public class NominetDomainExtension : Domain
	{
		public string FirstBill { get; set; }
		public string RecurBill { get; set; }
		public string AutoBill { get; set; }
		public string NextBill { get; set; }
		public string Reseller { get; set; }
		public string NextPeriod { get; set; }
		public string AutoPeriod { get; set; }
		public string RenewNotRequired { get; set; }
		public string RegStatus { get; set; }
		public List<string> Notes { get; set; }
	}
}