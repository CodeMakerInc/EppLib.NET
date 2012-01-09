// Copyright 2012 Code Maker Inc. (http://codemaker.net)
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System.Xml;

namespace EppLib.Entities
{
    public class HostInfoResponse : EppResponse
    {
        public HostInfoResponse(byte[] bytes) : base(bytes)
        {
        }

        protected override void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            namespaces.AddNamespace("host", "urn:ietf:params:xml:ns:host-1.0");

            var children = doc.SelectSingleNode("/ns:epp/ns:response/ns:resData/host:infData", namespaces);

            Host = new Host();

            if (children != null)
            {
                var nameNode = children.SelectSingleNode("host:name", namespaces);

                if (nameNode != null)
                {
                    Host.HostName = nameNode.InnerText;
                }

                var roidNode = children.SelectSingleNode("host:roid", namespaces);

                if (roidNode != null)
                {
                    Host.Roid = roidNode.InnerText;
                }

                var statusNodes = children.SelectNodes("host:status", namespaces);

                if (statusNodes != null)
                {
                    foreach (XmlNode statusNode in statusNodes)
                    {
                        if (statusNode.Attributes != null)
                        {
                            Host.Status.Add(new Status(statusNode.InnerText, statusNode.Attributes["s"].Value));
                        }
                    }
                    
                }

                var addresses = children.SelectNodes("host:addr", namespaces);

                if (addresses != null)
                {
                    foreach (XmlNode address in addresses)
                    {
                        var hostAddress = new HostAddress {IPAddress = address.InnerText};

                        if (address.Attributes != null)
                        {
                            hostAddress.IPVersion = address.Attributes["ip"].Value;
                        }

                        Host.Addresses.Add(hostAddress);
                    }
                }

                var clIdNode = children.SelectSingleNode("host:clID", namespaces);

                if (clIdNode != null)
                {
                    Host.ClId = clIdNode.InnerText;
                }

                var crIdNode = children.SelectSingleNode("host:crID", namespaces);

                if (crIdNode != null)
                {
                    Host.CrId = crIdNode.InnerText;
                }

                var crDateNode = children.SelectSingleNode("host:crDate", namespaces);

                if (crDateNode != null)
                {
                    Host.CrDate = crDateNode.InnerText;
                }

                var upIdNode = children.SelectSingleNode("host:upID", namespaces);

                if (upIdNode != null)
                {
                    Host.UpId = upIdNode.InnerText;
                }

                var upDateNode = children.SelectSingleNode("host:upDate", namespaces);

                if (upDateNode != null)
                {
                    Host.UpDate = upDateNode.InnerText;
                }

                var trDateNode = children.SelectSingleNode("host:trDate", namespaces);

                if (trDateNode != null)
                {
                    Host.TrDate = trDateNode.InnerText;
                }
            }
        
    }

        protected Host Host { get; set; }
    }
}
