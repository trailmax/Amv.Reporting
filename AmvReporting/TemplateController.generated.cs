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
    public partial class TemplateController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected TemplateController(Dummy d) { }

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
        public virtual System.Web.Mvc.ActionResult Update()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Update);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ViewRevisions()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ViewRevisions);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult CompareToLatest()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CompareToLatest);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public TemplateController Actions { get { return MVC.Template; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Template";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Template";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string Create = "Create";
            public readonly string Update = "Update";
            public readonly string ViewRevisions = "ViewRevisions";
            public readonly string CompareToLatest = "CompareToLatest";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string Create = "Create";
            public const string Update = "Update";
            public const string ViewRevisions = "ViewRevisions";
            public const string CompareToLatest = "CompareToLatest";
        }


        static readonly ActionParamsClass_Create s_params_Create = new ActionParamsClass_Create();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Create CreateParams { get { return s_params_Create; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Create
        {
            public readonly string command = "command";
        }
        static readonly ActionParamsClass_Update s_params_Update = new ActionParamsClass_Update();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Update UpdateParams { get { return s_params_Update; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Update
        {
            public readonly string id = "id";
            public readonly string command = "command";
        }
        static readonly ActionParamsClass_ViewRevisions s_params_ViewRevisions = new ActionParamsClass_ViewRevisions();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ViewRevisions ViewRevisionsParams { get { return s_params_ViewRevisions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ViewRevisions
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_CompareToLatest s_params_CompareToLatest = new ActionParamsClass_CompareToLatest();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_CompareToLatest CompareToLatestParams { get { return s_params_CompareToLatest; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_CompareToLatest
        {
            public readonly string id = "id";
            public readonly string revisionNumber = "revisionNumber";
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
                public readonly string _CommonScripts = "_CommonScripts";
                public readonly string _CreateEdit = "_CreateEdit";
                public readonly string CompareToLatest = "CompareToLatest";
                public readonly string Create = "Create";
                public readonly string Index = "Index";
                public readonly string Update = "Update";
                public readonly string ViewRevisions = "ViewRevisions";
            }
            public readonly string _CommonScripts = "~/Views/Template/_CommonScripts.cshtml";
            public readonly string _CreateEdit = "~/Views/Template/_CreateEdit.cshtml";
            public readonly string CompareToLatest = "~/Views/Template/CompareToLatest.cshtml";
            public readonly string Create = "~/Views/Template/Create.cshtml";
            public readonly string Index = "~/Views/Template/Index.cshtml";
            public readonly string Update = "~/Views/Template/Update.cshtml";
            public readonly string ViewRevisions = "~/Views/Template/ViewRevisions.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_TemplateController : AmvReporting.Controllers.TemplateController
    {
        public T4MVC_TemplateController() : base(Dummy.Instance) { }

        [NonAction]
        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Index()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            IndexOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void CreateOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Create()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Create);
            CreateOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void CreateOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, AmvReporting.Domain.Templates.Commands.CreateTemplateCommand command);

        [NonAction]
        public override System.Web.Mvc.ActionResult Create(AmvReporting.Domain.Templates.Commands.CreateTemplateCommand command)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Create);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "command", command);
            CreateOverride(callInfo, command);
            return callInfo;
        }

        [NonAction]
        partial void UpdateOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.Guid id);

        [NonAction]
        public override System.Web.Mvc.ActionResult Update(System.Guid id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Update);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            UpdateOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void UpdateOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, AmvReporting.Domain.Templates.Commands.UpdateTemplateCommand command);

        [NonAction]
        public override System.Web.Mvc.ActionResult Update(AmvReporting.Domain.Templates.Commands.UpdateTemplateCommand command)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Update);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "command", command);
            UpdateOverride(callInfo, command);
            return callInfo;
        }

        [NonAction]
        partial void ViewRevisionsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.Guid id);

        [NonAction]
        public override System.Web.Mvc.ActionResult ViewRevisions(System.Guid id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ViewRevisions);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ViewRevisionsOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void CompareToLatestOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.Guid id, int revisionNumber);

        [NonAction]
        public override System.Web.Mvc.ActionResult CompareToLatest(System.Guid id, int revisionNumber)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CompareToLatest);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "revisionNumber", revisionNumber);
            CompareToLatestOverride(callInfo, id, revisionNumber);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009
