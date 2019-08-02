using EppLib.Entities;

namespace EppLib.Extensions.Namestore
{
    public abstract class NamestoreExtension : EppExtension
    {
        private string _ns = "http://www.verisign-grs.com/epp/namestoreExt-1.1";
        protected override string Namespace
        {
            get { return _ns; }
            set { _ns = value; }
        }
    }
}
