using System;
using System.ComponentModel.DataAnnotations;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Models;

namespace AmvReporting.Commands
{
    public class CreateGraphCommand : ICommand
    {
        [Required]
        public String Title { get; set; }

        [Required]
        public String LinkName { get; set; }

        [Required]
        public ReportType ReportType { get; set; }

        [DataType(DataType.MultilineText)]
        public String Description { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public String Sql { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public String JavaScript { get; set; }

        [DataType(DataType.MultilineText)]
        public String Css { get; set; }
    }

    public class CreateGraphCommandHandler : ICommandHandler<CreateGraphCommand>
    {
        public void Handle(CreateGraphCommand command)
        {
            throw new NotImplementedException();
        }
    }
}