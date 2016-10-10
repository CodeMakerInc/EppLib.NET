using System;
using System.Xml;

namespace EppLib.Extensions.LaunchPhase.ClaimCheck
{
    public class ClaimKey
    {
        public string ValidatorId { get; set; }
        public string Value { get; set; }

        public ClaimKey()
        { }

        public ClaimKey(XmlNode node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            Value = node.InnerText;
            ValidatorId = node.Attributes["validatorID"]?.Value;
        }
    }
}
