using System.Xml;

namespace EppLib.Entities
{
    public class Logout : EppCommand<LogoutResponse>
    {
        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();

            var commandRootElement = GetCommandRootElement(doc);

            var logout = CreateElement(doc, "logout");
            commandRootElement.AppendChild(logout);
           
            return doc;
        }

        public override LogoutResponse FromBytes(byte[] bytes)
        {
            return new LogoutResponse(bytes);
        }
    }
}
