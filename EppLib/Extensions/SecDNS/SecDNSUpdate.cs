using EppLib.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace EppLib.Extensions.SecDNS
{
    /// <summary>
    /// The secDNS update extension (RFC 5910 section 5.2.5). Removals are processed before additions,
    /// so <see cref="RemoveAll"/> with new records replaces the domain's DNSSEC data.
    /// </summary>
    public class SecDNSUpdate : EppExtension
    {
        /// <summary>
        /// DS records to remove.
        /// </summary>
        public IList<SecDNSData> ToRemove { get; } = new List<SecDNSData>();

        /// <summary>
        /// DS records to add.
        /// </summary>
        public IList<SecDNSData> ToAdd { get; } = new List<SecDNSData>();

        /// <summary>
        /// DNSKEY records to remove.
        /// </summary>
        public IList<SecDNSKeyData> KeyDataToRemove { get; } = new List<SecDNSKeyData>();

        /// <summary>
        /// DNSKEY records to add.
        /// </summary>
        public IList<SecDNSKeyData> KeyDataToAdd { get; } = new List<SecDNSKeyData>();

        /// <summary>
        /// Removes all DS and DNSKEY data from the domain. Cannot be combined with <see cref="ToRemove"/> or <see cref="KeyDataToRemove"/>.
        /// </summary>
        public bool RemoveAll { get; set; }

        /// <summary>
        /// Changes the maximum signature lifetime, in seconds.
        /// </summary>
        public int? MaxSigLife { get; set; }

        /// <summary>
        /// Asks the registry to process the update with high priority. Registries may reject it if unsupported.
        /// </summary>
        public bool Urgent { get; set; }

        protected override string Namespace { get; set; }

        public override XmlNode ToXml(XmlDocument doc)
        {
            if (new[] { RemoveAll, ToRemove.Any(), KeyDataToRemove.Any() }.Count(x => x) > 1)
            {
                throw new InvalidOperationException("secDNS rem takes one of RemoveAll, ToRemove or KeyDataToRemove (RFC 5910 section 5.2.5).");
            }

            if (ToAdd.Any() && KeyDataToAdd.Any())
            {
                throw new InvalidOperationException("secDNS add takes either ToAdd or KeyDataToAdd, not both (RFC 5910 section 5.2.5).");
            }

            var root = doc.CreateElement("secDNS:update", SecDNSKeyData.NamespaceUri);
            root.SetAttribute("xmlns:secDNS", SecDNSKeyData.NamespaceUri);

            if (Urgent)
            {
                root.SetAttribute("urgent", "true");
            }

            var xsd = doc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xsd.Value = "urn:ietf:params:xml:ns:secDNS-1.1 secDNS-1.1.xsd";
            root.Attributes.Append(xsd);

            if (RemoveAll || ToRemove.Any() || KeyDataToRemove.Any())
            {
                var removeNode = doc.CreateElement("secDNS:rem", SecDNSKeyData.NamespaceUri);

                if (RemoveAll)
                {
                    var allNode = doc.CreateElement("secDNS:all", SecDNSKeyData.NamespaceUri);
                    allNode.InnerText = "true";
                    removeNode.AppendChild(allNode);
                }

                foreach (var data in ToRemove)
                {
                    removeNode.AppendChild(data.ToXml(doc));
                }

                foreach (var key in KeyDataToRemove)
                {
                    removeNode.AppendChild(key.ToXml(doc));
                }

                root.AppendChild(removeNode);
            }

            if (ToAdd.Any() || KeyDataToAdd.Any())
            {
                var addNode = doc.CreateElement("secDNS:add", SecDNSKeyData.NamespaceUri);

                foreach (var data in ToAdd)
                {
                    addNode.AppendChild(data.ToXml(doc));
                }

                foreach (var key in KeyDataToAdd)
                {
                    addNode.AppendChild(key.ToXml(doc));
                }

                root.AppendChild(addNode);
            }

            if (MaxSigLife.HasValue)
            {
                var chgNode = doc.CreateElement("secDNS:chg", SecDNSKeyData.NamespaceUri);
                var maxSigLifeNode = doc.CreateElement("secDNS:maxSigLife", SecDNSKeyData.NamespaceUri);
                maxSigLifeNode.InnerText = MaxSigLife.Value.ToString(CultureInfo.InvariantCulture);
                chgNode.AppendChild(maxSigLifeNode);
                root.AppendChild(chgNode);
            }

            return root;
        }
    }
}
