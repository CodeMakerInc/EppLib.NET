using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace EppLib.Extensions.LaunchPhase.ClaimCheck
{
    public class ClaimCheckResult
    {
        public string Name { get; set; }
        public bool ClaimExists { get; set; }
        public IList<ClaimKey> ClaimKeys { get; } = new List<ClaimKey>();

        public ClaimCheckResult()
        { }

        public ClaimCheckResult(XmlNode node, XmlNamespaceManager namespaces)
            : this()
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            // name node
            var nameNode = node.SelectSingleNode("launch:name", namespaces);
            if (nameNode != null)
            {
                Name = nameNode.InnerText;

                var existsValue = nameNode.Attributes?["exists"]?.Value?.ToLower(CultureInfo.InvariantCulture);
                ClaimExists = "1".Equals(existsValue) || "true".Equals(existsValue);
            }

            // claim key nodes
            var claimKeys = node.SelectNodes("launch:claimKey", namespaces);
            foreach (XmlNode claimKey in claimKeys)
            {
                ClaimKeys.Add(new ClaimKey(claimKey));
            }
        }
    }
}
