﻿using System;
using System.Collections.Generic;
using System.Linq;
using AmvReporting.Domain.ReportGroups.ViewModels;

namespace AmvReporting.Domain.ReportGroups
{
    public static class ReportGroupHelpers
    {
        public static String GetParentPath(IEnumerable<ReportGroup> allGroups, ReportGroup reportGroup)
        {
            if (reportGroup == null)
            {
                return String.Empty;
            }

            if (String.IsNullOrEmpty(reportGroup.ParentReportGroupId))
            {
                return reportGroup.Title;
            }

            var parent = allGroups.FirstOrDefault(g => g.Id == reportGroup.ParentReportGroupId);

            return GetParentPath(allGroups, parent) + " => " + reportGroup.Title;
        }


        public static String GetParentPath(IEnumerable<ReportGroup> allGroups, ReportGroupIndexViewModel reportGroup)
        {
            if (reportGroup == null)
            {
                return String.Empty;
            }

            if (String.IsNullOrEmpty(reportGroup.ParentReportGroupId))
            {
                return reportGroup.Title;
            }

            var parent = allGroups.FirstOrDefault(g => g.Id == reportGroup.ParentReportGroupId);

            return GetParentPath(allGroups, parent) + " => " + reportGroup.Title;
        }
    }
}