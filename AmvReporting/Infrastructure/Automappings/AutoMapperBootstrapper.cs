using AmvReporting.Models;
using AmvReporting.ViewModels;
using AutoMapper;

namespace AmvReporting.Infrastructure.Automappings
{
    public static class AutoMapperBootstrapper
    {
        public static void Initialize()
        {
            Mapper.CreateMap<Report, EditReportDetailsViewModel>();
        }
    }
}