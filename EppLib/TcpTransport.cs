using System;
using System.Net.Sockets;

using System.Text;
using System.Xml;
/**/
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;


/*
using OpenSSL;
using OpenSSL.Core;
using OpenSSL.X509;*/


namespace EppLib
{
    /// <summary>
    /// Encapsulates the TCP transport
    /// </summary>
    public class TcpTransport : IDisposable
    {
        private SslStream stream;

        private readonly string EPP_REGISTRY_COM;
        private readonly int PORT;

        private readonly bool loggingEnabled;
        private readonly X509Certificate clientCertificate;

        public TcpTransport(string host,int port, X509Certificate clientCertificate, bool loggingEnabled = false)
        {
            EPP_REGISTRY_COM = host;
            PORT = port;
            this.loggingEnabled = loggingEnabled;
            this.clientCertificate = clientCertificate;
        }

       /* public TcpTransport(bool loggingEnabled = false)
        {
            this.loggingEnabled = loggingEnabled;
            clientCertificate = new X509Certificate("isq.pfx", "tiburon");
        }

        public TcpTransport()
        {
            clientCertificate = new X509Certificate("isq.pfx", "tiburon");
        }*/

        /// <summary>
        /// Connect to the registry end point
        /// </summary>
        public void Connect()
        {
            var client = new TcpClient(EPP_REGISTRY_COM, PORT);

            // ** microsoft
            /* */
            stream = new SslStream(client.GetStream(), false, ValidateServerCertificate);

            //TODO: extract the cert
            var sslCert = clientCertificate;
            var clientCertificates = new X509CertificateCollection { sslCert };

            stream.AuthenticateAsClient(EPP_REGISTRY_COM, clientCertificates, SslProtocols.Ssl3, false);

            // ** openssl

            /* _sslStream = new SslStream(client.GetStream(), false, OpenSslValidateServerCertificate);

            var clientCertificates = new X509List(BIO.File(@"D:\isqsolutions\temp\isq.pem","r"));

//             var caCertificates = new X509Chain(BIO.File(@"D:\isqsolutions\temp\eppServer.pem", "r"));

             _sslStream.AuthenticateAsClient(EPP_REGISTRY_COM, null, null, SslProtocols.Ssl3, SslStrength.High, false);*/

           

        }

        /*private static bool OpenSslValidateServerCertificate(object sender, OpenSSL.X509.X509Certificate cert, OpenSSL.X509.X509Chain chain, int depth, VerifyResult result)
        {
            return true;
        }*/

        /**/
        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// Disconnect from the registry end point
        /// </summary>
        public void Disconnect()
        {
            stream.Close();
        }

        /// <summary>
        /// Read the command response
        /// </summary>
        /// <returns></returns>
        public byte[] Read()
        {
            var lenghtBytes = new byte[4];

            stream.Read(lenghtBytes, 0, 4);

            Array.Reverse(lenghtBytes);

            var length = BitConverter.ToInt32(lenghtBytes, 0) - 4;

            if (loggingEnabled)
            {
                Debug.Log("Reading " + length + " bytes.");
            }

            var bytes = new byte[length];

            var returned = 0;

            while (returned != length)
            {
                returned += stream.Read(bytes, returned, length - returned);
            }

            if (loggingEnabled)
            {
                Debug.Log("****************** Received ******************");
                Debug.Log(bytes);
            }

            return bytes;
        }

        /// <summary>
        /// Writes an XmlDocument to the transport stream
        /// </summary>
        /// <param name="s"></param>
        public void Write(XmlDocument s)
        {
            var bytes = GetBytes(s);

            var lenght = bytes.Length + 4;

            var lenghtBytes = BitConverter.GetBytes(lenght);
            Array.Reverse(lenghtBytes);

            stream.Write(lenghtBytes, 0, 4);

            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();

            if (loggingEnabled)
            {
                Debug.Log("****************** Sending ******************");
                Debug.Log(bytes);
            }

        }

        private static byte[] GetBytes(XmlDocument s)
        {
            return Encoding.UTF8.GetBytes(s.OuterXml);
        }

        public void Dispose()
        {
            if (stream!=null)
            {
                stream.Dispose();
            }
        }
    }
}
