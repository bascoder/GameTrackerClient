using System;
using System.Runtime.Serialization;

namespace GameTrackerClient
{
    public class GameTrackerException : Exception
    {
        public GameTrackerException()
        {
        }

        public GameTrackerException(string message) : base(message)
        {
        }

        public GameTrackerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GameTrackerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}