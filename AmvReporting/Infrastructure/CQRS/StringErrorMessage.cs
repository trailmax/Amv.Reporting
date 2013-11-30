using System;

namespace AmvReporting.Infrastructure.CQRS
{
    public class StringErrorMessage : IErrorMessage
    {
        public string FieldName { get; set; }
        public string Message { get; set; }

        public StringErrorMessage(String message)
        {
            FieldName = "";
            Message = message;
        }

        public StringErrorMessage(String fieldName, String message)
        {
            FieldName = fieldName;
            Message = message;
        }


        public override string ToString()
        {
            return Message;
        }
    }
}