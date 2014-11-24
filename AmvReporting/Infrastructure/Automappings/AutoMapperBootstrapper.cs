using AmvReporting.Domain.ReportGroups;
using AmvReporting.Domain.ReportGroups.ViewModels;
using AmvReporting.Domain.Reports;
using AmvReporting.Domain.Reports.ViewModels;
using AutoMapper;


namespace AmvReporting.Infrastructure.Automappings
{
    public static class AutoMapperBootstrapper
    {
        public static void Initialize()
        {
            Mapper.CreateMap<ReportAggregate, ReportDetailsViewModel>()
                  .ForMember(d => d.AggregateId, o => o.MapFrom(s => s.Id));
            Mapper.CreateMap<ReportAggregate, EditReportDetailsViewModel>()
                  .ForMember(d => d.AggregateId, o => o.MapFrom(s => s.Id));

            Mapper.CreateMap<ReportViewModel, ReportIndexViewModel>()
                  .ForMember(d => d.Id, o => o.Ignore())
                  .ForMember(d => d.GroupFullName, o => o.Ignore());

            Mapper.CreateMap<ReportGroup, ReportGroupViewModel>();

            Mapper.CreateMap<ReportGroup, ReportGroupIndexViewModel>()
                  .ForMember(d => d.ParentFullName, o => o.Ignore());

            Mapper.CreateMap<ReportAggregate, ReportViewModel>()
                  .ForMember(d => d.AggregateId, o => o.MapFrom(s => s.Id));

            Mapper.AddGlobalIgnore("Possible");
        }
    }
}