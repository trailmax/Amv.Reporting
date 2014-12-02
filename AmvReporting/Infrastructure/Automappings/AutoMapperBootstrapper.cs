using AmvReporting.Domain.DatabaseConnections;
using AmvReporting.Domain.DatabaseConnections.Commands;
using AmvReporting.Domain.ReportGroups;
using AmvReporting.Domain.ReportGroups.Commands;
using AmvReporting.Domain.ReportingConfigs;
using AmvReporting.Domain.ReportingConfigs.Commands;
using AmvReporting.Domain.Reports;
using AmvReporting.Domain.Reports.Commands;
using AmvReporting.Domain.Templates;
using AmvReporting.Domain.Templates.Commands;
using AutoMapper;


namespace AmvReporting.Infrastructure.Automappings
{
    public static class AutoMapperBootstrapper
    {
        public static void Initialize()
        {
            Mapper.CreateMap<ReportAggregate, UpdateReportMetadataCommand>()
                  .ForMember(d => d.AggregateId, o => o.MapFrom(s => s.Id));

            Mapper.CreateMap<ReportAggregate, UpdateReportCodeCommand>()
                  .ForMember(d => d.AggregateId, o => o.MapFrom(s => s.Id));

            Mapper.CreateMap<ReportAggregate, ReportViewModel>()
                  .ForMember(d => d.AggregateId, o => o.MapFrom(s => s.Id));


            Mapper.CreateMap<ReportGroup, EditReportGroupCommand>();

            Mapper.CreateMap<ReportingConfig, UpdateConfigurationCommand>();

            Mapper.CreateMap<DatabaseConnection, EditDatabaseDetailsCommand>();


            Mapper.CreateMap<TemplateAggregate, UpdateTemplateCommand>()
                .ForMember(d => d.AggregateId, o => o.MapFrom(s => s.Id));


            Mapper.AddGlobalIgnore("Possible");
        }
    }
}