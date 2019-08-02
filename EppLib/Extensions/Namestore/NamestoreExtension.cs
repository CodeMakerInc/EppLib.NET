using EppLib.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

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
