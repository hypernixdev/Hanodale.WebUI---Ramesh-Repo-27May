using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.BusinessLogic; 

namespace Hanodale.BusinessLogic
{
    public class UserProfileService : IUserProfileService
    {
        #region UserProfile

        public Hanodale.DataAccessLayer.Interfaces.IUserProfileService DataProvider;

        public UserProfileService()
        {
            this.DataProvider = new Hanodale.DataAccessLayer.Services.UserProfileService();
        }

        public UserProfileDetails GetUserProfile(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            if (string.IsNullOrEmpty(search))
                return this.DataProvider.GetUserProfile(currentUserId, userId, startIndex, pageSize);
            else
                return this.DataProvider.GetUserProfileBySearch(currentUserId, userId, startIndex, pageSize, search);
        }

        public UserProfiles SaveUserProfile(int currentUserId, UserProfiles userProfileEn, string pageName)
        {
            if (userProfileEn.id > 0) 
                return this.DataProvider.UpdateUserProfile(currentUserId, userProfileEn, pageName);
            else
                return this.DataProvider.CreateUserProfile(currentUserId, userProfileEn, pageName);
        }

        public bool DeleteUserProfile(int currentUserId, int userProfileId, string pageName)
        {
            return this.DataProvider.DeleteUserProfile(currentUserId, userProfileId, pageName);
        }

        public UserProfiles GetUserProfileById(int id)
        {
            return this.DataProvider.GetUserProfileById(id);
        }

        public bool IsUserProfileExists(UserProfiles userProfile)
        {
            return this.DataProvider.IsUserProfileExists(userProfile);
        }

        //public List<UserProfiles> GetUserProfileList()
        //{
        //    return this.DataProvider.GetUserProfileList();
        //}

        #endregion

       
    }
}
