﻿using System;
using System.Collections.Generic;
using AmvReporting.Domain.Reports;


namespace AmvReporting.Domain.Menus
{
    public class MenuModel
    {
        public List<ReportViewModel> TopLevelReports { get; set; }
        public List<MenuNode> MenuNodes { get; set; }
    }


    public class MenuNode
    {
        public String ReportGroupParentId { get; set; }
        public String ReportGroupId { get; set; }
        public String ReportGroupTitle { get; set; }
        public bool Enabled { get; set; }
        public List<ReportViewModel> Reports { get; set; }
        public List<MenuNode> MenuNodes { get; set; }
    }
}