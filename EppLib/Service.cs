using System.Xml;
using EppLib.Entities;

namespace EppLib
{
    /// <summary>
    /// Encapsulates the EPP protocol
    /// </summary>
    public class Service
    {
        private readonly TcpTransport transport;
        
        public Service(TcpTransport transport)
        {
            this.transport = transport;
        }


        /// <summary>
        /// Connects to the registry end point
        /// </summary>
        public void Connect()
        {
            transport.Connect();
            transport.Read();

        }

        /// <summary>
        /// Executes an EPP command
        /// </summary>
        /// <param name="command">The EPP command</param>
        /// <returns></returns>
        public T Execute<T>(EppBase<T> command) where T:EppResponse
        {
           byte[] bytes = SendAndReceive(command.ToXml());

            return command.FromBytes(bytes);
        }

        internal byte[] SendAndReceive(XmlDocument xmlDocument)
        {
            transport.Write(xmlDocument);

            return transport.Read();
        }

        /// <summary>
        /// Disconects from the registry end point
        /// </summary>
        public void Disconnect()
        {
            transport.Disconnect();
        }
    }
}
