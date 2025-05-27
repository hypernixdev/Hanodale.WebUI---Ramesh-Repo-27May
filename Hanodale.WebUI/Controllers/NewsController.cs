using Hanodale.BusinessLogic;
using Hanodale.WebUI.Authentication;
using Hanodale.WebUI.Helpers;
using Hanodale.WebUI.Models;
using Microsoft.Practices.ServiceLocation;
using System.ServiceModel;
using System.Web.Mvc;
using System.Linq;
using Hanodale.Utility.Globalize;
using Hanodale.WebUI.Logging.Elmah;
using System;
using Hanodale.Domain.DTOs;
using System.Collections.Generic;
using System.Web.Configuration;

namespace Hanodale.WebUI.Controllers
{
    public partial class NewsController : AuthorizedController
    {
        #region Declaration
        const string PAGE_URL = "News/Index";
        #endregion

        #region Constructor

        private readonly INewsService svc; private readonly ICommonService svcCommon;

        public NewsController(INewsService _bLService, ICommonService _commonService)
            
        {
            this.svc = _bLService; this.svcCommon = _commonService;
        }
        #endregion

        #region News Details

        [AppAuthorize]
        public virtual ActionResult Index()
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.News.Views.Index, _accessRight)
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_VIEW
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        status = Common.Status.Denied.ToString(),
                        message = Resources.NO_ACCESS_RIGHTS
                    });
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        [HttpPost]
        [AppAuthorize]
        public virtual JsonResult News()
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView)
                    {
                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.News.Views.Index, _accessRight)
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_VIEW
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        status = Common.Status.Denied.ToString(),
                        message = Resources.NO_ACCESS_RIGHTS
                    });
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        [Authorize]
        public virtual ActionResult BindNews(DataTableModel param)
        {
            int totalRecordCount = 0;
            IEnumerable<Newss> filteredNews = null;
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        // Get login user Id
                        int userId = this.CurrentUserId;

                        var scheduleModel = this.svc.GetNews(this.CurrentUserId, param.iFilterAct, userId, param.iDisplayStart, param.iDisplayLength, param.sSearch, null);

                        if (svc != null)
                        {
                            //Sorting
                            var sortColumnIndex = param.iSortCol_0;
                            Func<Newss, string> orderingFunction = (c => sortColumnIndex == 0 ? c.description :
                                                            sortColumnIndex == 1 ? c.loggedBy : c.loggedDate.ToString()
                                                            );

                            filteredNews = scheduleModel.lstNews;
                            if (param.sSortDir_0 != null)
                            {
                                if (param.sSortDir_0 == "asc")
                                    filteredNews = filteredNews.OrderBy(orderingFunction);
                                else
                                    filteredNews = filteredNews.OrderByDescending(orderingFunction);
                            }
                            if (param.iSortCol_0 == 3)
                            {
                                if (param.sSortDir_0 == "asc")
                                    filteredNews = filteredNews.OrderBy(p => p.loggedDate);
                                else
                                    filteredNews = filteredNews.OrderByDescending(p => p.loggedDate);
                            }

                            var result = NewsData(filteredNews, this.CurrentUserId);
                            return Json(new
                            {
                                sEcho = param.sEcho,
                                iTotalRecords = scheduleModel.recordDetails.totalRecords,
                                iTotalDisplayRecords = scheduleModel.recordDetails.totalDisplayRecords,
                                aaData = result
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new
                            {
                                status = Common.Status.Error.ToString(),
                                message = Resources.MSG_ERR_SERVICE
                            });
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_EDIT
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        status = Common.Status.Denied.ToString(),
                        message = Resources.NO_ACCESS_RIGHTS
                    });
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        /// <summary>
        /// This method is to get the user data as string array to bind into datatbale
        /// </summary>
        /// <param name="userEntry">User list</param>
        /// <returns></returns>
        public static List<string[]> NewsData(IEnumerable<Newss> scheduledEntry, int currentUserId)
        {
            return scheduledEntry.Select(entry => new string[]
            {  
                entry.description,
                entry.loggedBy,
                entry.loggedDate.GetValueOrDefault().ToString("dd/MM/yyyy"),
                Common.Encrypt(currentUserId.ToString(), entry.id.ToString())
            }).ToList();
        }

        [Authorize]
        [HttpPost] public virtual JsonResult RenderAction()
        {

            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                return Json(new
                {
                    viewMarkup = Common.RenderPartialViewToString(this, MVC.News.Views.RenderAction, _accessRight)
                });
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        #endregion

        #region Add,Edit and Delete

        [Authorize]
        public virtual JsonResult Create()
        {
            try
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                NewsModel _model = new NewsModel();
                _model.isEdit = false;

                if (_accessRight != null)
                {
                    if (_accessRight.canView && _accessRight.canAdd)
                    {

                        _model.id = Common.Encrypt(this.CurrentUserId.ToString(), "0");
                        _model.loggedBy = this.UserName;
                        _model.loggedDate = DateTime.Now;

                        return Json(new
                        {
                            viewMarkup = Common.RenderPartialViewToString(this, MVC.News.Views.Create, _model)
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_ADD
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        status = Common.Status.Denied.ToString(),
                        message = Resources.NO_ACCESS_RIGHTS
                    });
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public virtual JsonResult SaveNews(NewsModel model)
        {
            if (ModelState.IsValid)
            {
                AccessRightsModel _accessRight = new AccessRightsModel();
                try
                {
                    _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                    if (_accessRight != null)
                    {
                        int newId = 0;
                        newId = Common.DecryptToID(this.CurrentUserId.ToString(), model.id);
                        if (newId > 0)
                        {
                            if (!_accessRight.canEdit)
                            {
                                return Json(new
                                {
                                    status = "Access " + Common.Status.Denied.ToString(),
                                    message = Resources.NO_ACCESS_RIGHTS_EDIT
                                });
                            }
                        }
                        else
                        {
                            if (!_accessRight.canAdd)
                            {
                                return Json(new
                                {
                                    status = "Access " + Common.Status.Denied.ToString(),
                                    message = Resources.NO_ACCESS_RIGHTS_ADD
                                });
                            }
                        }

                        if (svc != null)
                        {
                            Newss entity = new Newss();
                            entity.description = model.description;
                            entity.loggedBy = this.UserName;
                            entity.loggedDate = DateTime.Now;
                            entity.modifiedBy = this.UserName;
                            entity.modifiedDate = DateTime.Now;

                            if (newId > 0)
                            {
                                entity.id = newId;
                            }
                            else
                            {
                                entity.createdBy = this.UserName;
                                entity.createdDate = DateTime.Now;
                            }
                            bool isExists = svc.IsNewsExists(entity);
                            if (!isExists)
                            {
                                var save = svc.SaveNews(this.CurrentUserId, entity, _accessRight.pageName);

                                if (save != null)
                                {
                                    if (newId > 0)
                                    {
                                        return Json(new
                                        {
                                            status = Common.Status.Success.ToString(),
                                            message = Resources.MSG_UPDATE,
                                            id = Common.Encrypt(this.CurrentUserId.ToString(), save.id.ToString()),

                                        });
                                    }
                                    else
                                    {
                                        return Json(new
                                        {
                                            status = Common.Status.Success.ToString(),
                                            message = Resources.MSG_SAVE,
                                            id = Common.Encrypt(this.CurrentUserId.ToString(), save.id.ToString()),

                                        });
                                    }
                                }
                                else
                                {
                                    if (newId > 0)
                                    {
                                        return Json(new
                                        {
                                            status = Common.Status.Success.ToString(),
                                            message = Resources.MSG_ERR_UPDATE
                                        });
                                    }
                                    else
                                    {
                                        return Json(new
                                        {
                                            status = Common.Status.Error.ToString(),
                                            message = Resources.MSG_ERR_SAVE
                                        });
                                    }
                                }
                            }
                            else
                            {
                                return Json(new
                                {

                                    status = Common.Status.Warning.ToString(),
                                    message = Resources.NEWS_RECORD_EXISTS
                                });
                            }
                        }
                        else
                        {
                            return Json(new
                            {
                                status = Common.Status.Error.ToString(),
                                message = Resources.MSG_ERR_SERVICE
                            });
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS
                        });
                    }
                }
                catch (Exception err)
                {
                    throw new ErrorException(err.Message);
                }
            }
            return Json(new
            {
                status = Common.Status.Error.ToString(),
                message = Resources.MSG_ERR_INVALIDMODEL
            });
        }

        [HttpPost]
        [Authorize]
        public virtual JsonResult Edit(string id, bool readOnly)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            NewsModel _model = new NewsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    int newId = 0;
                    newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                    _model.readOnly = readOnly;
                    if (_accessRight.canView || _accessRight.canEdit)
                    {
                        if (svc != null)
                        {
                            var scheduled = svc.GetNewsById(newId);

                            if (scheduled != null)
                            {

                                _model.isEdit = true;
                                _model.id = id;
                                _model.description = scheduled.description;
                                _model.loggedBy = this.UserName;
                                _model.loggedDate = DateTime.Now;

                            }
                            else
                            {
                                return Json(new
                                {
                                    status = Common.Status.Success.ToString(),
                                    message = Resources.MSG_ERR_RETRIEVE
                                });
                            }
                        }
                        else
                        {
                            return Json(new
                            {
                                status = Common.Status.Error.ToString(),
                                message = Resources.MSG_ERR_SERVICE
                            });
                        }
                    }
                    else
                    {
                        //Redirect to access denied page
                        if (!_accessRight.canView)
                        {
                            return Json(new
                            {
                                status = Common.Status.Denied.ToString(),
                                message = Resources.NO_ACCESS_RIGHTS_VIEW
                            });
                        }
                        if (!_accessRight.canEdit)
                        {
                            return Json(new
                            {
                                status = Common.Status.Denied.ToString(),
                                message = Resources.NO_ACCESS_RIGHTS_EDIT
                            });
                        }
                    }
                }
            }
            catch (Exception err)
            {
                throw new ErrorException(err.Message);
            }

            return Json(new
            {
                viewMarkup = Common.RenderPartialViewToString(this, MVC.News.Views.Create, _model)
            });
        }

        [HttpPost]
        [Authorize]
        public virtual ActionResult Delete(string id)
        {
            AccessRightsModel _accessRight = new AccessRightsModel();
            try
            {
                _accessRight = Common.GetUserRights(this.CurrentUserId, PAGE_URL);

                if (_accessRight != null)
                {
                    if (_accessRight.canView && _accessRight.canDelete)
                    {
                        int newId = 0;
                        newId = Common.DecryptToID(this.CurrentUserId.ToString(), id);
                        if (svc != null)
                        {
                            bool isSuccess = svc.DeleteNews(this.CurrentUserId, newId, _accessRight.pageName);
                            if (isSuccess)
                            {
                                return Json(new
                                {
                                    status = Common.Status.Success.ToString(),
                                    message = Resources.MSG_DELETE
                                });
                            }
                            else
                            {
                                return Json(new
                                {
                                    status = Common.Status.Error.ToString(),
                                    message = Resources.MSG_ERR_DELETE
                                });
                            }
                        }
                        else
                        {
                            return Json(new
                            {
                                status = Common.Status.Error.ToString(),
                                message = Resources.MSG_ERR_SERVICE
                            });
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            status = Common.Status.Denied.ToString(),
                            message = Resources.NO_ACCESS_RIGHTS_DELETE
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("REFERENCE"))
                {
                    return Json(new
                    {
                        status = Common.Status.Warning.ToString(),
                        message = Resources.MSG_RECORD_IN_USE
                    });
                }
                else
                {
                    throw new ErrorException(ex.Message);
                }
            }
            return View();
        }

        #endregion
    }
}
