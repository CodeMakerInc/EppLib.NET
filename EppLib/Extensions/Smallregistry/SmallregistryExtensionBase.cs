using EppLib.Entities;

namespace EppLib.Extensions.Smallregistry
{
    /// <summary>
    /// Base class for registry : smallregistry
    /// </summary>
    public abstract class SmallregistryExtensionBase : EppExtension
    {
        private string _ns = "https://www.smallregistry.net/schemas/sr-1.0.xsd";
        protected override string Namespace
        {
            get { return _ns; }
            set { _ns = value; }
        }
    }
}
