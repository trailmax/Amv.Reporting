using System;

namespace AmvReporting.Domain
{
    public class DomainException : Exception
    {
        public DomainException(string message)
            : base(message)
        {
        }

        public DomainException(string formattedString, params object[] args)
            : base(String.Format(formattedString, args))
        {

        }
    }
}