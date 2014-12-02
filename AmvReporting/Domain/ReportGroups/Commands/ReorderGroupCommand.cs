using System;
using System.Linq;
using AmvReporting.Domain.Reports;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;
using Raven.Client;

namespace AmvReporting.Domain.ReportGroups.Commands
{
    public class ReorderGroupCommand : ICommand
    {
        public String ParentGroupId { get; set; }

        public Guid[] ReportIds { get; set; }
        public String[] GroupIds { get; set; }
    }



    public class ReorderGroupCommandHandler : ICommandHandler<ReorderGroupCommand>
    {
        private readonly IDocumentSession ravenSession;
        private readonly IRepository repository;

        public ReorderGroupCommandHandler(IDocumentSession ravenSession, IRepository repository)
        {
            this.ravenSession = ravenSession;
            this.repository = repository;
        }


        public void Handle(ReorderGroupCommand command)
        {
            if (command.ReportIds == null)
            {
                command.ReportIds = new Guid[0];
            }
            for (var i = 0; i < command.ReportIds.Count(); i++)
            {
                var report = repository.GetById<ReportAggregate>(command.ReportIds[i]);
                if (report != null)
                {
                    report.SetListOrder(i);
                    repository.Save(report, Guid.NewGuid());
                }
            }

            if (command.GroupIds == null)
            {
                command.GroupIds = new string[0];
            }
            for (var i = 0; i < (command.GroupIds).Count(); i++)
            {
                var group = ravenSession.Load<ReportGroup>(command.GroupIds[i]);
                if (group != null)
                {
                    group.ListOrder = i;
                }
            }


            ravenSession.SaveChanges();
        }
    }
}