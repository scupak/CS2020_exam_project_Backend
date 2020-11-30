using System;
using System.Runtime.Serialization;

namespace Core.Entities.Exceptions
{
    public class DataBaseException : Exception
    {
        public DataBaseException()
        {
        }

        protected DataBaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DataBaseException(string? message) : base(message)
        {
        }

        public DataBaseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}