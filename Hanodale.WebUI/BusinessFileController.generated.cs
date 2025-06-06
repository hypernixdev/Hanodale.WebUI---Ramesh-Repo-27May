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
    public partial class BusinessFileController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected BusinessFileController(Dummy d) { }

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
        public virtual System.Web.Mvc.ActionResult BindBusinessFile()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.BindBusinessFile);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult RenderAction()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.RenderAction);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Create()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Create);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult SaveBusinessFile()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.SaveBusinessFile);
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
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult DownloadBusinessFile()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DownloadBusinessFile);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public BusinessFileController Actions { get { return MVC.BusinessFile; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "BusinessFile";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "BusinessFile";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string BusinessFile = "BusinessFile";
            public readonly string BindBusinessFile = "BindBusinessFile";
            public readonly string RenderAction = "RenderAction";
            public readonly string Create = "Create";
            public readonly string SaveBusinessFile = "SaveBusinessFile";
            public readonly string Edit = "Edit";
            public readonly string Delete = "Delete";
            public readonly string DownloadBusinessFile = "DownloadBusinessFile";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string BusinessFile = "BusinessFile";
            public const string BindBusinessFile = "BindBusinessFile";
            public const string RenderAction = "RenderAction";
            public const string Create = "Create";
            public const string SaveBusinessFile = "SaveBusinessFile";
            public const string Edit = "Edit";
            public const string Delete = "Delete";
            public const string DownloadBusinessFile = "DownloadBusinessFile";
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
        static readonly ActionParamsClass_BindBusinessFile s_params_BindBusinessFile = new ActionParamsClass_BindBusinessFile();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_BindBusinessFile BindBusinessFileParams { get { return s_params_BindBusinessFile; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_BindBusinessFile
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
        static readonly ActionParamsClass_SaveBusinessFile s_params_SaveBusinessFile = new ActionParamsClass_SaveBusinessFile();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SaveBusinessFile SaveBusinessFileParams { get { return s_params_SaveBusinessFile; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SaveBusinessFile
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
        }
        static readonly ActionParamsClass_Delete s_params_Delete = new ActionParamsClass_Delete();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Delete DeleteParams { get { return s_params_Delete; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Delete
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_DownloadBusinessFile s_params_DownloadBusinessFile = new ActionParamsClass_DownloadBusinessFile();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_DownloadBusinessFile DownloadBusinessFileParams { get { return s_params_DownloadBusinessFile; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_DownloadBusinessFile
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
                public readonly string _Create = "_Create";
                public readonly string _Index = "_Index";
                public readonly string _RenderAction = "_RenderAction";
            }
            public readonly string _Create = "~/Views/BusinessFile/_Create.cshtml";
            public readonly string _Index = "~/Views/BusinessFile/_Index.cshtml";
            public readonly string _RenderAction = "~/Views/BusinessFile/_RenderAction.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_BusinessFileController : Hanodale.WebUI.Controllers.BusinessFileController
    {
        public T4MVC_BusinessFileController() : base(Dummy.Instance) { }

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
        partial void BusinessFileOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult BusinessFile()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.BusinessFile);
            BusinessFileOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void BindBusinessFileOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Hanodale.WebUI.Models.DataTableModel param, string myKey);

        [NonAction]
        public override System.Web.Mvc.ActionResult BindBusinessFile(Hanodale.WebUI.Models.DataTableModel param, string myKey)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.BindBusinessFile);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "param", param);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "myKey", myKey);
            BindBusinessFileOverride(callInfo, param, myKey);
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
        partial void CreateOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string id);

        [NonAction]
        public override System.Web.Mvc.ActionResult Create(string id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Create);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            CreateOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void SaveBusinessFileOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, Hanodale.WebUI.Models.BusinessFileModel model);

        [NonAction]
        public override System.Web.Mvc.JsonResult SaveBusinessFile(Hanodale.WebUI.Models.BusinessFileModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.SaveBusinessFile);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            SaveBusinessFileOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void EditOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, string id);

        [NonAction]
        public override System.Web.Mvc.JsonResult Edit(string id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.Edit);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            EditOverride(callInfo, id);
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

        [NonAction]
        partial void DownloadBusinessFileOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string id);

        [NonAction]
        public override System.Web.Mvc.ActionResult DownloadBusinessFile(string id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DownloadBusinessFile);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            DownloadBusinessFileOverride(callInfo, id);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
