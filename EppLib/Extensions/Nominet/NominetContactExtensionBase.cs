using EppLib.Entities;

namespace EppLib.Extensions.Nominet
{
	public abstract class NominetContactExtensionBase : EppExtension
	{
		private string _ns = "http://www.nominet.org.uk/epp/xml/contact-nom-ext-1.0";
		protected override string Namespace
		{
			get { return _ns; }
			set { _ns = value; }
		}
	}
}