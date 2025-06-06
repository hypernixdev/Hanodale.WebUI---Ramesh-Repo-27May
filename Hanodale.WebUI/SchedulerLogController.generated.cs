// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591
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
namespace Hanodale.WebUI.Controllers
{
    public partial class SchedulerLogController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected SchedulerLogController(Dummy d) { }

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
        public virtual System.Web.Mvc.ActionResult Index()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult BindSchedulerLog()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.BindSchedulerLog);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult RenderAction()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.RenderAction);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult Create()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.Create);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult SaveSchedulerLog()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.SaveSchedulerLog);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult Edit()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.Edit);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Delete()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Delete);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public SchedulerLogController Actions { get { return MVC.SchedulerLog; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "SchedulerLog";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "SchedulerLog";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string BindSchedulerLog = "BindSchedulerLog";
            public readonly string RenderAction = "RenderAction";
            public readonly string Create = "Create";
            public readonly string SaveSchedulerLog = "SaveSchedulerLog";
            public readonly string Edit = "Edit";
            public readonly string Delete = "Delete";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string BindSchedulerLog = "BindSchedulerLog";
            public const string RenderAction = "RenderAction";
            public const string Create = "Create";
            public const string SaveSchedulerLog = "SaveSchedulerLog";
            public const string Edit = "Edit";
            public const string Delete = "Delete";
        }


        static readonly ActionParamsClass_Index s_params_Index = new ActionParamsClass_Index();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Index IndexParams { get { return s_params_Index; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Index
        {
            public readonly string id = "id";
            public readonly string readOnly = "readOnly";
        }
        static readonly ActionParamsClass_BindSchedulerLog s_params_BindSchedulerLog = new ActionParamsClass_BindSchedulerLog();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_BindSchedulerLog BindSchedulerLogParams { get { return s_params_BindSchedulerLog; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_BindSchedulerLog
        {
            public readonly string param = "param";
            public readonly string myKey = "myKey";
        }
        static readonly ActionParamsClass_RenderAction s_params_RenderAction = new ActionParamsClass_RenderAction();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_RenderAction RenderActionParams { get { return s_params_RenderAction; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_RenderAction
        {
            public readonly string readOnly = "readOnly";
        }
        static readonly ActionParamsClass_Create s_params_Create = new ActionParamsClass_Create();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Create CreateParams { get { return s_params_Create; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Create
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_SaveSchedulerLog s_params_SaveSchedulerLog = new ActionParamsClass_SaveSchedulerLog();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SaveSchedulerLog SaveSchedulerLogParams { get { return s_params_SaveSchedulerLog; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SaveSchedulerLog
        {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_Edit s_params_Edit = new ActionParamsClass_Edit();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Edit EditParams { get { return s_params_Edit; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Edit
        {
            public readonly string id = "id";
            public readonly string readOnly = "readOnly";
        }
        static readonly ActionParamsClass_Delete s_params_Delete = new ActionParamsClass_Delete();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Delete DeleteParams { get { return s_params_Delete; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Delete
        {
            public readonly string id = "id";
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
                public readonly string Create = "Create";
                public readonly string Index = "Index";
                public readonly string RenderAction = "RenderAction";
            }
            public readonly string Create = "~/Views/SchedulerLog/Create.cshtml";
            public readonly string Index = "~/Views/SchedulerLog/Index.cshtml";
            public readonly string RenderAction = "~/Views/SchedulerLog/RenderAction.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_SchedulerLogController : Hanodale.WebUI.Controllers.SchedulerLogController
    {
        public T4MVC_SchedulerLogController() : base(Dummy.Instance) { }

        [NonAction]
        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string id, bool readOnly);

        [NonAction]
        public override System.Web.Mvc.ActionResult Index(string id, bool readOnly)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "readOnly", readOnly);
            IndexOverride(callInfo, id, readOnly);
            return callInfo;
        }

        [NonAction]
        partial void BindSchedulerLogOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Hanodale.WebUI.Models.DataTableModel param, string myKey);

        [NonAction]
        public override System.Web.Mvc.ActionResult BindSchedulerLog(Hanodale.WebUI.Models.DataTableModel param, string myKey)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.BindSchedulerLog);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "param", param);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "myKey", myKey);
            BindSchedulerLogOverride(callInfo, param, myKey);
            return callInfo;
        }

        [NonAction]
        partial void RenderActionOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, bool readOnly);

        [NonAction]
        public override System.Web.Mvc.JsonResult RenderAction(bool readOnly)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.RenderAction);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "readOnly", readOnly);
            RenderActionOverride(callInfo, readOnly);
            return callInfo;
        }

        [NonAction]
        partial void CreateOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, string id);

        [NonAction]
        public override System.Web.Mvc.JsonResult Create(string id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.Create);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            CreateOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void SaveSchedulerLogOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, Hanodale.WebUI.Models.SchedulerLogModel model);

        [NonAction]
        public override System.Web.Mvc.JsonResult SaveSchedulerLog(Hanodale.WebUI.Models.SchedulerLogModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.SaveSchedulerLog);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            SaveSchedulerLogOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void EditOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, string id, bool readOnly);

        [NonAction]
        public override System.Web.Mvc.JsonResult Edit(string id, bool readOnly)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.Edit);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "readOnly", readOnly);
            EditOverride(callInfo, id, readOnly);
            return callInfo;
        }

        [NonAction]
        partial void DeleteOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string id);

        [NonAction]
        public override System.Web.Mvc.ActionResult Delete(string id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Delete);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            DeleteOverride(callInfo, id);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
