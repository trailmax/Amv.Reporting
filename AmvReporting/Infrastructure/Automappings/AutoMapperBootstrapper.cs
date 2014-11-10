﻿using AmvReporting.Domain.ReportGroups;
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
            Mapper.CreateMap<ReportViewModel, ReportDetailsViewModel>()
                .ForMember(d => d.RedirectingId, o => o.Ignore());

            Mapper.CreateMap<ReportViewModel, EditReportDetailsViewModel>()
                .ForMember(d => d.RedirectingId, o => o.Ignore());

            Mapper.CreateMap<ReportViewModel, ReportIndexViewModel>()
                .ForMember(d => d.GroupFullName, o => o.Ignore());

            Mapper.CreateMap<ReportGroup, ReportGroupViewModel>();

            Mapper.CreateMap<ReportGroup, ReportGroupIndexViewModel>()
                .ForMember(d => d.ParentFullName, o => o.Ignore());

            Mapper.AddGlobalIgnore("Possible");
        }
    }
}