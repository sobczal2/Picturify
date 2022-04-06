using System;

namespace Sobczal.Picturify.Core.Processing.Exceptions
{
    public class ParamsArgumentException : Exception
    {
        public ParamsArgumentException() : base()
        {
        }

        public ParamsArgumentException(string message) : base(message)
        {
        }

        public ParamsArgumentException(string argName, string message) : base($"Invalid {argName}: {message}")
        {
        }
    }
}