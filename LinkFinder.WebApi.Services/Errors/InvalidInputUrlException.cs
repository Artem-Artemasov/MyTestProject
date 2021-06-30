using System;
using System.Runtime.Serialization;

namespace LinkFinder.WebApi.Logic.Errors
{
    public class InvalidInputUrlException : Exception
    {
        public InvalidInputUrlException()
        {
        }

        public InvalidInputUrlException(string message) : base(message)
        {
        }

        public InvalidInputUrlException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidInputUrlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
