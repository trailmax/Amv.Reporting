using System;

namespace AmvReporting.Infrastructure.CQRS
{
    public interface IErrorMessage
    {
        String FieldName { get; set; }

        String ToString();
    }
}