using System;
using System.Runtime.Serialization;

namespace EppLib.Entities
{
    [Serializable]
    public class EppException : Exception
    {
        public EppException(string msg):base(msg)
        {
        
        }

        public EppException()
        {
        }

        public EppException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EppException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}