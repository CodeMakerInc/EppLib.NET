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
