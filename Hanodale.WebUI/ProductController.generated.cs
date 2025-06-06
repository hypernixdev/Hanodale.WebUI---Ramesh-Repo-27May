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
    public partial class ProductController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected ProductController(Dummy d) { }

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
        public virtual System.Web.Mvc.ActionResult BindProduct()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.BindProduct);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult AddProduct()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AddProduct);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult EditProduct()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.EditProduct);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult SaveProduct()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.SaveProduct);
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
        public virtual System.Web.Mvc.JsonResult GetProductByPartNum()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetProductByPartNum);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult GetProductStockBalance()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetProductStockBalance);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult SearchProducts()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.SearchProducts);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult GetProductPrices()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetProductPrices);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult SearchOrderUOM()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.SearchOrderUOM);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult GetremarksComplimentary()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetremarksComplimentary);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult GetOperationStyleRemarks()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetOperationStyleRemarks);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult ProductPanelDetail()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ProductPanelDetail);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ProductController Actions { get { return MVC.Product; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Product";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Product";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string Product = "Product";
            public readonly string BindProduct = "BindProduct";
            public readonly string RenderAction = "RenderAction";
            public readonly string Create = "Create";
            public readonly string AddProduct = "AddProduct";
            public readonly string EditProduct = "EditProduct";
            public readonly string SaveProduct = "SaveProduct";
            public readonly string Edit = "Edit";
            public readonly string GetCustomSearchPanel = "GetCustomSearchPanel";
            public readonly string Delete = "Delete";
            public readonly string GetProductByPartNum = "GetProductByPartNum";
            public readonly string GetProductStockBalance = "GetProductStockBalance";
            public readonly string SearchProducts = "SearchProducts";
            public readonly string GetProductPrices = "GetProductPrices";
            public readonly string SyncProducts = "SyncProducts";
            public readonly string syncUOMConv = "syncUOMConv";
            public readonly string SyncStockBalances = "SyncStockBalances";
            public readonly string SyncBrands = "SyncBrands";
            public readonly string SearchOrderType = "SearchOrderType";
            public readonly string SearchOpertionalStyle = "SearchOpertionalStyle";
            public readonly string SearchOrderUOM = "SearchOrderUOM";
            public readonly string SearchComplimentary = "SearchComplimentary";
            public readonly string GetremarksComplimentary = "GetremarksComplimentary";
            public readonly string GetOperationStyleRemarks = "GetOperationStyleRemarks";
            public readonly string ProductPanelDetail = "ProductPanelDetail";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string Product = "Product";
            public const string BindProduct = "BindProduct";
            public const string RenderAction = "RenderAction";
            public const string Create = "Create";
            public const string AddProduct = "AddProduct";
            public const string EditProduct = "EditProduct";
            public const string SaveProduct = "SaveProduct";
            public const string Edit = "Edit";
            public const string GetCustomSearchPanel = "GetCustomSearchPanel";
            public const string Delete = "Delete";
            public const string GetProductByPartNum = "GetProductByPartNum";
            public const string GetProductStockBalance = "GetProductStockBalance";
            public const string SearchProducts = "SearchProducts";
            public const string GetProductPrices = "GetProductPrices";
            public const string SyncProducts = "SyncProducts";
            public const string syncUOMConv = "syncUOMConv";
            public const string SyncStockBalances = "SyncStockBalances";
            public const string SyncBrands = "SyncBrands";
            public const string SearchOrderType = "SearchOrderType";
            public const string SearchOpertionalStyle = "SearchOpertionalStyle";
            public const string SearchOrderUOM = "SearchOrderUOM";
            public const string SearchComplimentary = "SearchComplimentary";
            public const string GetremarksComplimentary = "GetremarksComplimentary";
            public const string GetOperationStyleRemarks = "GetOperationStyleRemarks";
            public const string ProductPanelDetail = "ProductPanelDetail";
        }


        static readonly ActionParamsClass_BindProduct s_params_BindProduct = new ActionParamsClass_BindProduct();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_BindProduct BindProductParams { get { return s_params_BindProduct; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_BindProduct
        {
            public readonly string param = "param";
            public readonly string myKey = "myKey";
        }
        static readonly ActionParamsClass_AddProduct s_params_AddProduct = new ActionParamsClass_AddProduct();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_AddProduct AddProductParams { get { return s_params_AddProduct; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_AddProduct
        {
            public readonly string parCode = "parCode";
            public readonly string description = "description";
            public readonly string prodGroup = "prodGroup";
            public readonly string id = "id";
            public readonly string partNum = "partNum";
        }
        static readonly ActionParamsClass_EditProduct s_params_EditProduct = new ActionParamsClass_EditProduct();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_EditProduct EditProductParams { get { return s_params_EditProduct; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_EditProduct
        {
            public readonly string parCode = "parCode";
            public readonly string description = "description";
            public readonly string prodGroup = "prodGroup";
            public readonly string id = "id";
            public readonly string partNum = "partNum";
            public readonly string QtyType_ModuleItem_Id = "QtyType_ModuleItem_Id";
            public readonly string orderQty = "orderQty";
            public readonly string OrderUOM_Id = "OrderUOM_Id";
            public readonly string operationStyle_ModuleItem_Id = "operationStyle_ModuleItem_Id";
            public readonly string operationCost = "operationCost";
            public readonly string complimentary_ModuleItem_Id = "complimentary_ModuleItem_Id";
            public readonly string unitPrice = "unitPrice";
        }
        static readonly ActionParamsClass_SaveProduct s_params_SaveProduct = new ActionParamsClass_SaveProduct();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SaveProduct SaveProductParams { get { return s_params_SaveProduct; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SaveProduct
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
        static readonly ActionParamsClass_GetProductByPartNum s_params_GetProductByPartNum = new ActionParamsClass_GetProductByPartNum();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GetProductByPartNum GetProductByPartNumParams { get { return s_params_GetProductByPartNum; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GetProductByPartNum
        {
            public readonly string partNum = "partNum";
            public readonly string shipToId = "shipToId";
            public readonly string customerId = "customerId";
            public readonly string orderDate = "orderDate";
        }
        static readonly ActionParamsClass_GetProductStockBalance s_params_GetProductStockBalance = new ActionParamsClass_GetProductStockBalance();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GetProductStockBalance GetProductStockBalanceParams { get { return s_params_GetProductStockBalance; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GetProductStockBalance
        {
            public readonly string partNum = "partNum";
            public readonly string uom = "uom";
        }
        static readonly ActionParamsClass_SearchProducts s_params_SearchProducts = new ActionParamsClass_SearchProducts();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SearchProducts SearchProductsParams { get { return s_params_SearchProducts; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SearchProducts
        {
            public readonly string param = "param";
        }
        static readonly ActionParamsClass_GetProductPrices s_params_GetProductPrices = new ActionParamsClass_GetProductPrices();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GetProductPrices GetProductPricesParams { get { return s_params_GetProductPrices; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GetProductPrices
        {
            public readonly string request = "request";
        }
        static readonly ActionParamsClass_SearchOrderUOM s_params_SearchOrderUOM = new ActionParamsClass_SearchOrderUOM();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SearchOrderUOM SearchOrderUOMParams { get { return s_params_SearchOrderUOM; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SearchOrderUOM
        {
            public readonly string orderTypeId = "orderTypeId";
        }
        static readonly ActionParamsClass_GetremarksComplimentary s_params_GetremarksComplimentary = new ActionParamsClass_GetremarksComplimentary();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GetremarksComplimentary GetremarksComplimentaryParams { get { return s_params_GetremarksComplimentary; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GetremarksComplimentary
        {
            public readonly string complimentaryId = "complimentaryId";
        }
        static readonly ActionParamsClass_GetOperationStyleRemarks s_params_GetOperationStyleRemarks = new ActionParamsClass_GetOperationStyleRemarks();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GetOperationStyleRemarks GetOperationStyleRemarksParams { get { return s_params_GetOperationStyleRemarks; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GetOperationStyleRemarks
        {
            public readonly string operationStyleId = "operationStyleId";
        }
        static readonly ActionParamsClass_ProductPanelDetail s_params_ProductPanelDetail = new ActionParamsClass_ProductPanelDetail();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ProductPanelDetail ProductPanelDetailParams { get { return s_params_ProductPanelDetail; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ProductPanelDetail
        {
            public readonly string partNum = "partNum";
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
                public readonly string _SearchPanel = "_SearchPanel";
                public readonly string AddProduct = "AddProduct";
                public readonly string Create = "Create";
                public readonly string Index = "Index";
                public readonly string Maintenance = "Maintenance";
                public readonly string RenderAction = "RenderAction";
            }
            public readonly string _SearchPanel = "~/Views/Product/_SearchPanel.cshtml";
            public readonly string AddProduct = "~/Views/Product/AddProduct.cshtml";
            public readonly string Create = "~/Views/Product/Create.cshtml";
            public readonly string Index = "~/Views/Product/Index.cshtml";
            public readonly string Maintenance = "~/Views/Product/Maintenance.cshtml";
            public readonly string RenderAction = "~/Views/Product/RenderAction.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_ProductController : Hanodale.WebUI.Controllers.ProductController
    {
        public T4MVC_ProductController() : base(Dummy.Instance) { }

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
        partial void ProductOverride(T4MVC_System_Web_Mvc_JsonResult callInfo);

        [NonAction]
        public override System.Web.Mvc.JsonResult Product()
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.Product);
            ProductOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void BindProductOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Hanodale.WebUI.Models.DataTableModel param, string myKey);

        [NonAction]
        public override System.Web.Mvc.ActionResult BindProduct(Hanodale.WebUI.Models.DataTableModel param, string myKey)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.BindProduct);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "param", param);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "myKey", myKey);
            BindProductOverride(callInfo, param, myKey);
            return callInfo;
        }

        [NonAction]
        partial void RenderActionOverride(T4MVC_System_Web_Mvc_JsonResult callInfo);

        [NonAction]
        public override System.Web.Mvc.JsonResult RenderAction()
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.RenderAction);
            RenderActionOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void CreateOverride(T4MVC_System_Web_Mvc_JsonResult callInfo);

        [NonAction]
        public override System.Web.Mvc.JsonResult Create()
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.Create);
            CreateOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void AddProductOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string parCode, string description, string prodGroup, int id, string partNum);

        [NonAction]
        public override System.Web.Mvc.ActionResult AddProduct(string parCode, string description, string prodGroup, int id, string partNum)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AddProduct);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "parCode", parCode);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "description", description);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "prodGroup", prodGroup);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "partNum", partNum);
            AddProductOverride(callInfo, parCode, description, prodGroup, id, partNum);
            return callInfo;
        }

        [NonAction]
        partial void EditProductOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string parCode, string description, string prodGroup, int id, string partNum, int QtyType_ModuleItem_Id, int orderQty, int OrderUOM_Id, int operationStyle_ModuleItem_Id, float operationCost, int complimentary_ModuleItem_Id, float unitPrice);

        [NonAction]
        public override System.Web.Mvc.ActionResult EditProduct(string parCode, string description, string prodGroup, int id, string partNum, int QtyType_ModuleItem_Id, int orderQty, int OrderUOM_Id, int operationStyle_ModuleItem_Id, float operationCost, int complimentary_ModuleItem_Id, float unitPrice)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.EditProduct);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "parCode", parCode);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "description", description);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "prodGroup", prodGroup);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "partNum", partNum);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "QtyType_ModuleItem_Id", QtyType_ModuleItem_Id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "orderQty", orderQty);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "OrderUOM_Id", OrderUOM_Id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "operationStyle_ModuleItem_Id", operationStyle_ModuleItem_Id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "operationCost", operationCost);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "complimentary_ModuleItem_Id", complimentary_ModuleItem_Id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "unitPrice", unitPrice);
            EditProductOverride(callInfo, parCode, description, prodGroup, id, partNum, QtyType_ModuleItem_Id, orderQty, OrderUOM_Id, operationStyle_ModuleItem_Id, operationCost, complimentary_ModuleItem_Id, unitPrice);
            return callInfo;
        }

        [NonAction]
        partial void SaveProductOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, Hanodale.WebUI.Models.ProductModel model);

        [NonAction]
        public override System.Web.Mvc.JsonResult SaveProduct(Hanodale.WebUI.Models.ProductModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.SaveProduct);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            SaveProductOverride(callInfo, model);
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
        partial void GetCustomSearchPanelOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult GetCustomSearchPanel()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GetCustomSearchPanel);
            GetCustomSearchPanelOverride(callInfo);
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
        partial void GetProductByPartNumOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, string partNum, string shipToId, string customerId, string orderDate);

        [NonAction]
        public override System.Web.Mvc.JsonResult GetProductByPartNum(string partNum, string shipToId, string customerId, string orderDate)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetProductByPartNum);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "partNum", partNum);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "shipToId", shipToId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "customerId", customerId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "orderDate", orderDate);
            GetProductByPartNumOverride(callInfo, partNum, shipToId, customerId, orderDate);
            return callInfo;
        }

        [NonAction]
        partial void GetProductStockBalanceOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, string partNum, string uom);

        [NonAction]
        public override System.Web.Mvc.JsonResult GetProductStockBalance(string partNum, string uom)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetProductStockBalance);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "partNum", partNum);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "uom", uom);
            GetProductStockBalanceOverride(callInfo, partNum, uom);
            return callInfo;
        }

        [NonAction]
        partial void SearchProductsOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, Hanodale.Domain.DTOs.Order.ProductDatatableFilter param);

        [NonAction]
        public override System.Web.Mvc.JsonResult SearchProducts(Hanodale.Domain.DTOs.Order.ProductDatatableFilter param)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.SearchProducts);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "param", param);
            SearchProductsOverride(callInfo, param);
            return callInfo;
        }

        [NonAction]
        partial void GetProductPricesOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, Hanodale.SyncService.Models.ProductPriceRequest request);

        [NonAction]
        public override System.Web.Mvc.JsonResult GetProductPrices(Hanodale.SyncService.Models.ProductPriceRequest request)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetProductPrices);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "request", request);
            GetProductPricesOverride(callInfo, request);
            return callInfo;
        }

        [NonAction]
        partial void SyncProductsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult SyncProducts()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SyncProducts);
            SyncProductsOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void syncUOMConvOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult syncUOMConv()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.syncUOMConv);
            syncUOMConvOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void SyncStockBalancesOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult SyncStockBalances()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SyncStockBalances);
            SyncStockBalancesOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void SyncBrandsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult SyncBrands()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SyncBrands);
            SyncBrandsOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void SearchOrderTypeOverride(T4MVC_System_Web_Mvc_JsonResult callInfo);

        [NonAction]
        public override System.Web.Mvc.JsonResult SearchOrderType()
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.SearchOrderType);
            SearchOrderTypeOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void SearchOpertionalStyleOverride(T4MVC_System_Web_Mvc_JsonResult callInfo);

        [NonAction]
        public override System.Web.Mvc.JsonResult SearchOpertionalStyle()
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.SearchOpertionalStyle);
            SearchOpertionalStyleOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void SearchOrderUOMOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, int orderTypeId);

        [NonAction]
        public override System.Web.Mvc.JsonResult SearchOrderUOM(int orderTypeId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.SearchOrderUOM);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "orderTypeId", orderTypeId);
            SearchOrderUOMOverride(callInfo, orderTypeId);
            return callInfo;
        }

        [NonAction]
        partial void SearchComplimentaryOverride(T4MVC_System_Web_Mvc_JsonResult callInfo);

        [NonAction]
        public override System.Web.Mvc.JsonResult SearchComplimentary()
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.SearchComplimentary);
            SearchComplimentaryOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void GetremarksComplimentaryOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, int complimentaryId);

        [NonAction]
        public override System.Web.Mvc.JsonResult GetremarksComplimentary(int complimentaryId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetremarksComplimentary);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "complimentaryId", complimentaryId);
            GetremarksComplimentaryOverride(callInfo, complimentaryId);
            return callInfo;
        }

        [NonAction]
        partial void GetOperationStyleRemarksOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, int operationStyleId);

        [NonAction]
        public override System.Web.Mvc.JsonResult GetOperationStyleRemarks(int operationStyleId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.GetOperationStyleRemarks);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "operationStyleId", operationStyleId);
            GetOperationStyleRemarksOverride(callInfo, operationStyleId);
            return callInfo;
        }

        [NonAction]
        partial void ProductPanelDetailOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, string partNum);

        [NonAction]
        public override System.Web.Mvc.JsonResult ProductPanelDetail(string partNum)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ProductPanelDetail);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "partNum", partNum);
            ProductPanelDetailOverride(callInfo, partNum);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
