using System;
using System.Collections.Generic;
using System.Linq;

namespace AmvReporting.Infrastructure.CQRS
{
    /// <summary>
    /// ErrorList simply inherits List of IErrorMessage
    /// It allows us to override ToString() so that we can debug error messages easily in tests 
    /// It provide a ToString implementation for Error message and it will be included in ToString
    /// when called on ErrorList
    /// </summary>
    public class ErrorList : List<StringErrorMessage>
    {
        public override string ToString()
        {
            var result = String.Join(", ", this);
            return result;
        }

        public bool IsValid()
        {
            return !this.Any();
        }

        public void Add(string fieldName, string message)
        {
            this.Add(new StringErrorMessage(fieldName, message));
        }

        public void Add(String message)
        {
            this.Add(new StringErrorMessage(message));
        }
    }
}