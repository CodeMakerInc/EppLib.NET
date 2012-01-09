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
    public class Login : EppCommand<LoginResponse>
    {
        private readonly string clientId;
        private readonly string password;
        
        public Options Options;

        public Login(string clientId, string password)
        {
            this.clientId = clientId;
            this.password = password;
        }

        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();
            
            var commandRootElement = GetCommandRootElement(doc);

            var login = CreateElement(doc,"login");

            AddXmlElement(doc, login, "clID", clientId);
            AddXmlElement(doc, login, "pw", password);
            
           
            if (Options != null)
            {
                var options_element = CreateElement(doc,"options");

                if (Options.MVersion != null)
                {
                    AddXmlElement(doc, options_element, "version", Options.MVersion);
                }
                if (Options.MLang != null)
                {
                    AddXmlElement(doc, options_element, "lang", Options.MLang);
                }
                login.AppendChild(options_element);
            }

            commandRootElement.AppendChild(login);

            return doc;
        }

        public override LoginResponse FromBytes(byte[] bytes)
        {
            return new LoginResponse(bytes);
        }
    }
}
