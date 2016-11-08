using EppLib.Entities;
using System.Collections.Generic;
using System.Xml;

namespace EppLib.Extensions.LaunchPhase.ClaimCreate
{
    public class ClaimCreateExtension : EppExtension
    {
        private IList<ClaimNotice> notices;

        public ClaimCreateExtension(IList<ClaimNotice> notices)
        {
            this.notices = notices;
        }

        public IList<ClaimNotice> Notices { get; } = new List<ClaimNotice>();

        protected override string Namespace { get; set; }

        public override XmlNode ToXml(XmlDocument doc)
        {
            var root = doc.CreateElement("launch:create", "urn:ietf:params:xml:ns:launch-1.0");

            var xsd = doc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xsd.Value = "urn:ietf:params:xml:ns:launch-1.0 launch-1.0.xsd";
            root.Attributes.Append(xsd);

            root.SetAttribute("xmlns:launch", "urn:ietf:params:xml:ns:launch-1.0");

            root.InnerXml = "<launch:phase>claims</launch:phase>";

            foreach (var notice in notices)
            {
                var launchNoticeNode = doc.CreateElement("launch:notice", "urn:ietf:params:xml:ns:launch-1.0");

                var noticeIdNode = doc.CreateElement("launch:noticeID", "urn:ietf:params:xml:ns:launch-1.0");
                noticeIdNode.InnerText = notice.NoticeId;
                if (!string.IsNullOrEmpty(notice.ValidatorId)) noticeIdNode.SetAttribute("validatorID", notice.ValidatorId); 
                launchNoticeNode.AppendChild(noticeIdNode);

                var notAfterNode = doc.CreateElement("launch:notAfter", "urn:ietf:params:xml:ns:launch-1.0");
                notAfterNode.InnerText = notice.NotAfter.ToUniversalTime().ToString("o");
                launchNoticeNode.AppendChild(notAfterNode);

                var acceptedDateNode = doc.CreateElement("launch:acceptedDate", "urn:ietf:params:xml:ns:launch-1.0");
                acceptedDateNode.InnerText = notice.AcceptedDate.ToUniversalTime().ToString("o");
                launchNoticeNode.AppendChild(acceptedDateNode);

                root.AppendChild(launchNoticeNode);
            }

            return root;
        }
    }
}
