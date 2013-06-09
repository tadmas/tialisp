using System;

namespace TiaLisp
{
    public class LispException : Exception
    {
        public LispException()
        {
        }

        public LispException(string message) : base(message)
        {
        }

        public LispException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
