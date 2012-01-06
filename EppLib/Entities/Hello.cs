using System.Xml;

namespace EppLib.Entities
{
    public class Hello : EppBase<HelloResponse>
    {
        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();
            var root = CreateDocRoot(doc);
            var hello = CreateElement(doc,"hello");
            root.AppendChild(hello);
            doc.AppendChild(root);

            return doc;

        }
        
        public override HelloResponse FromBytes(byte[] bytes)
        {
            return new HelloResponse(bytes);
        }
    }
}
