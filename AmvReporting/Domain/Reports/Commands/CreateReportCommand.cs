﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Domain.Reports.Commands
{
    public class CreateReportCommand : ICommand
    {
        [Required]
        public String Title { get; set; }

        [Required]
        public ReportType ReportType { get; set; }

        public String ReportGroupId { get; set; }

        [DataType(DataType.MultilineText)]
        public String Description { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public String Sql { get; set; }

        [DataType(DataType.MultilineText)]
        public String JavaScript { get; set; }

        [DataType(DataType.MultilineText)]
        public String Css { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public String HtmlOverride { get; set; }


        [Required]
        public String DatabaseId { get; set; }

        public bool Enabled { get; set; }
    }
}