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
using System.IO;
using System.Text;
using System.Xml;

namespace EppLib.Entities
{
    public class EppResponse
    {
		public EppResponse(string xml)
		{
			FromXmlString(Encoding.UTF8.GetBytes(xml));
		}

    	public EppResponse(byte[] bytes)
        {
            FromXmlString(bytes);
        }

        private void FromXmlString(byte[] bytes)
        {
            var doc = new XmlDocument();
            
            var namespaces = new XmlNamespaceManager(doc.NameTable);

            namespaces.AddNamespace("ns", "urn:ietf:params:xml:ns:epp-1.0");

            doc.Load(new MemoryStream(bytes));

            var resultNode = doc.SelectSingleNode("/ns:epp/ns:response/ns:result",namespaces);

            if (resultNode != null)
            {
                ProcessResultNode(doc, namespaces, resultNode);
            }

            ProcessDataNode(doc, namespaces);

            var extension = doc.SelectSingleNode("/ns:epp/ns:response/ns:extension", namespaces);

            if (extension != null)
            {
                ProcessExtensionNode(doc, namespaces);
            }

        	Xml = doc.OuterXml;
        }

		protected virtual void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            //default implementation does nothing
        }

		protected virtual void ProcessExtensionNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            //default implementation does nothing
        }

        private void ProcessResultNode(XmlDocument doc, XmlNamespaceManager namespaces, XmlNode resultNode)
        {
            if (resultNode.Attributes != null) Code = resultNode.Attributes["code"].Value;

            var msgNode = doc.SelectSingleNode("/ns:epp/ns:response/ns:result/ns:msg", namespaces);

            if (msgNode != null) {Message = msgNode.InnerText;}

            var extValueNode = doc.SelectSingleNode("/ns:epp/ns:response/ns:result/ns:extValue", namespaces);

            if (extValueNode != null)
            {
                var valueNode = extValueNode.SelectSingleNode("ns:value", namespaces);
                if (valueNode != null) ExtValue = valueNode.InnerText;

                var reasonNode = extValueNode.SelectSingleNode("ns:reason", namespaces);
                if (reasonNode != null) Reason = reasonNode.InnerText;
            }
        }

        public string Reason { get; private set; }
        public string ExtValue { get; private set; }
        public string Message { get; private set; }
        public string Code { get; private set; }
		public string Xml { get; private set; }
    }

}
