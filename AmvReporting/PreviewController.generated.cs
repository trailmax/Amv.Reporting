// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
#pragma warning disable 1591, 3008, 3009
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace AmvReporting.Controllers
{
    public partial class PreviewController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected PreviewController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Data()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Data);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Report()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Report);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult CleanseAndFormatSql()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CleanseAndFormatSql);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public PreviewController Actions { get { return MVC.Preview; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Preview";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Preview";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Data = "Data";
            public readonly string Report = "Report";
            public readonly string CleanseAndFormatSql = "CleanseAndFormatSql";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Data = "Data";
            public const string Report = "Report";
            public const string CleanseAndFormatSql = "CleanseAndFormatSql";
        }


        static readonly ActionParamsClass_Data s_params_Data = new ActionParamsClass_Data();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Data DataParams { get { return s_params_Data; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Data
        {
            public readonly string aggregateId = "aggregateId";
            public readonly string sql = "sql";
        }
        static readonly ActionParamsClass_Report s_params_Report = new ActionParamsClass_Report();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Report ReportParams { get { return s_params_Report; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Report
        {
            public readonly string query = "query";
        }
        static readonly ActionParamsClass_CleanseAndFormatSql s_params_CleanseAndFormatSql = new ActionParamsClass_CleanseAndFormatSql();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_CleanseAndFormatSql CleanseAndFormatSqlParams { get { return s_params_CleanseAndFormatSql; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_CleanseAndFormatSql
        {
            public readonly string sql = "sql";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string Data = "Data";
                public readonly string Report = "Report";
            }
            public readonly string Data = "~/Views/Preview/Data.cshtml";
            public readonly string Report = "~/Views/Preview/Report.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_PreviewController : AmvReporting.Controllers.PreviewController
    {
        public T4MVC_PreviewController() : base(Dummy.Instance) { }

        [NonAction]
        partial void DataOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.Guid aggregateId, string sql);

        [NonAction]
        public override System.Web.Mvc.ActionResult Data(System.Guid aggregateId, string sql)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Data);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "aggregateId", aggregateId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "sql", sql);
            DataOverride(callInfo, aggregateId, sql);
            return callInfo;
        }

        [NonAction]
        partial void ReportOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, AmvReporting.Domain.Preview.Queries.PreviewReportQuery query);

        [NonAction]
        public override System.Web.Mvc.ActionResult Report(AmvReporting.Domain.Preview.Queries.PreviewReportQuery query)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Report);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "query", query);
            ReportOverride(callInfo, query);
            return callInfo;
        }

        [NonAction]
        partial void CleanseAndFormatSqlOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string sql);

        [NonAction]
        public override System.Web.Mvc.ActionResult CleanseAndFormatSql(string sql)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CleanseAndFormatSql);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "sql", sql);
            CleanseAndFormatSqlOverride(callInfo, sql);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009
