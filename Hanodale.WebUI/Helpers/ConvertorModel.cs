using Hanodale.Domain.DTOs;
using Hanodale.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hanodale.WebUI.Helpers
{
    public class ConvertorModel
    {
        public static MainMenuModel MainMenu(Menu model)
        {
            if (model != null)
            {
                return new MainMenuModel { id = model.id, imageUrl = model.imageUrl, menuName = model.menuName, pageName = model.pageName, pageUrl = model.pageUrl, status = model.status, subMenus = SubMenu(model.subMenus) };
            }
            else
                return null;
        }

        public static List<MainMenuModel> MainMenu(List<Menu> ListModel)
        {
            List<MainMenuModel> result = new List<MainMenuModel>();
            foreach (var item in ListModel)
            {
                if (item != null)
                {
                    result.Add(MainMenu(item));
                }

            }
            return result;
        }

        public static SubMenuModel SubMenu(SubMenus model)
        {
            if (model != null)
            {
                return new SubMenuModel { id = model.id, imageUrl = model.imageUrl, isMainMenu = model.isMainMenu, pageName = model.pageName, pageUrl = model.pageUrl, mainMenu_Id = model.mainMenu_Id, reportCategory_Id = model.reportCategory_Id, subMenuName = model.subMenuName, userRights = UserRight(model.userRights) };
            }
            else
                return null;
        }

        public static List<SubMenuModel> SubMenu(List<SubMenus> ListModel)
        {
            List<SubMenuModel> result = new List<SubMenuModel>();
            foreach (var item in ListModel)
            {
                if (item != null)
                {
                    result.Add(SubMenu(item));
                }

            }
            return result;
        }

        public static UserRightsModel UserRight(UserRights model)
        {
            if (model != null)
            {
                return new UserRightsModel { id = model.id, canAdd = model.canAdd, canDelete = model.canDelete, canEdit = model.canEdit, canView = model.canView, subMenu_Id = model.subMenus!=null?model.subMenus.id:0, userRole_Id = model.userRole_Id};
            }
            else
                return null;
        }

        public static List<UserRightsModel> UserRight(List<UserRights> ListModel)
        {
            List<UserRightsModel> result = new List<UserRightsModel>();
            foreach (var item in ListModel)
            {
                if (item != null)
                {
                    result.Add(UserRight(item));
                }

            }
            return result;
        }
    }
}