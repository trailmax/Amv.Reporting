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
            Mapper.AddGlobalIgnore("Possible");
        }
    }
}