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
    public class Poll : EppCommand<PollResponse>
    {
        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();

            var commandRootElement = GetCommandRootElement(doc);
            
            var poll = CreateElement(doc, "poll");

            poll.SetAttribute("op", Type);

            if (Type.Equals("ack") && MessageId!=null)
            {
                poll.SetAttribute("msgID", MessageId);
            }

            commandRootElement.AppendChild(poll);

            return doc;
        }

        public string MessageId { get; set; }
        public string Type { get; set; }

        public override PollResponse FromBytes(byte[] bytes)
        {
            return new PollResponse(bytes);
        }
    }
}
