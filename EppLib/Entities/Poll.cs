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
