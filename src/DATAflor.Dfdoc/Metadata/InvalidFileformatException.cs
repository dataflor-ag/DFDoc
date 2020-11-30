using System;
using System.Runtime.Serialization;

namespace DATAflor.Dfdoc.Metadata
{
    public class InvalidFileformatException : Exception
    {
        public InvalidFileformatException()
        {
        }

        protected InvalidFileformatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InvalidFileformatException(string message) : base(message)
        {
        }

        public InvalidFileformatException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}