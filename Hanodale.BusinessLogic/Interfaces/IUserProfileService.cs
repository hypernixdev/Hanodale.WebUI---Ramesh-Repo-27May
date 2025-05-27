using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.BusinessLogic
{
    public interface IUserProfileService
    {
        #region UserProfile

        UserProfileDetails GetUserProfile(int currentUserId, int userId, int startIndex, int pageSize, string search);

        UserProfiles SaveUserProfile(int currentUserId, UserProfiles userProfileEn, string pageName); 

        bool DeleteUserProfile(int currentUserId, int userProfileId, string pageName);

        UserProfiles GetUserProfileById(int id);

        bool IsUserProfileExists(UserProfiles userProfile);

        //List<UserProfiles> GetUserProfileList();

        #endregion
    }
}
