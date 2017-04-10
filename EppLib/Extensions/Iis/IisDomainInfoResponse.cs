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

using System;
using System.Collections.Generic;
using System.Xml;
using EppLib.Entities;

namespace EppLib.Extensions.Iis
{
    public class IisDomainInfoResponse : DomainInfoResponse
    {
        private IisDomainExtension _iisDomain = new IisDomainExtension();
        public virtual new IisDomainExtension Domain
        {
            get { return _iisDomain; }
        }

        public IisDomainInfoResponse(byte[] bytes) : base(bytes) { }

        protected override void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            base.ProcessDataNode(doc, namespaces);

            _iisDomain.ClId = _domain.ClId;
            _iisDomain.Contacts = _domain.Contacts;
            _iisDomain.CrDate = _domain.CrDate;
            _iisDomain.CrId = _domain.CrId;
            _iisDomain.ExDate = _domain.ExDate;
            _iisDomain.Hosts = _domain.Hosts;
            _iisDomain.Name = _domain.Name;
            _iisDomain.NameServers = _domain.NameServers;
            _iisDomain.Password = _domain.Password;
            _iisDomain.RegistrantId = _domain.RegistrantId;
            _iisDomain.Roid = _domain.Roid;
            _iisDomain.Status = _domain.Status;
            _iisDomain.TrDate = _domain.TrDate;
            _iisDomain.UpDate = _domain.UpDate;
            _iisDomain.UpId = _domain.UpId;
        }

        protected override void ProcessExtensionNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            //namespaces.AddNamespace("domain", "urn:ietf:params:xml:ns:domain-1.0");
            namespaces.AddNamespace("iis", "urn:se:iis:xml:epp:iis-1.2");

            var children = doc.SelectSingleNode("/ns:epp/ns:response/ns:extension/iis:infData", namespaces);

            if (children != null)
            {
                XmlNode node;

                // State
                node = children.SelectSingleNode("iis:state", namespaces);
                if (node != null)
                {
                    _iisDomain.State = node.InnerText;
                }

                // ClientDelete
                _iisDomain.ClientDelete = false;

                node = children.SelectSingleNode("iis:clientDelete", namespaces);
                if (node != null)
                {
                    var innerText = node.InnerText;

                    if (!String.IsNullOrEmpty(innerText))
                    {

                        if(innerText.Equals("1") || innerText.ToLowerInvariant().Equals("true"))
                        {
                            _iisDomain.ClientDelete = true;
                        }
                    }
                    
                }

                // DeactivationDate
                node = children.SelectSingleNode("iis:deactDate", namespaces);
                if (node != null)
                {
                    _iisDomain.DeactivationDate = node.InnerText;
                }

                // DeleteDate
                node = children.SelectSingleNode("iis:delDate", namespaces);
                if (node != null)
                {
                    _iisDomain.DeleteDate = node.InnerText;
                }

                // ReleaseDate
                node = children.SelectSingleNode("iis:relDate", namespaces);
                if (node != null)
                {
                    _iisDomain.ReleaseDate = node.InnerText;
                }
            }
        }
    }
}