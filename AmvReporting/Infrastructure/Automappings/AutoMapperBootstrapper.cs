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
            Mapper.CreateMap<Report, EditReportDetailsViewModel>();
            Mapper.CreateMap<Report, ReportIndexViewModel>()
                .ForMember(d => d.GroupFullName, o => o.Ignore());

            Mapper.CreateMap<ReportGroup, ReportGroupViewModel>();
            Mapper.CreateMap<ReportGroup, ReportGroupIndexViewModel>()
                .ForMember(d => d.ParentFullName, o => o.Ignore());

            Mapper.AddGlobalIgnore("Possible");
        }
    }
}