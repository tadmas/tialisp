using System;

namespace TiaLisp.Execution
{
    public class SignatureMismatchException : LispException
    {
        public SignatureMismatchException(string message) : base(message)
        {
        }
    }
}
