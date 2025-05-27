using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanodale.Entity.Core;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Xml;
using System.ServiceModel;
using System.Data.Objects.SqlClient;
using System.Collections;
using System.Globalization;
using Hanodale.Domain.DTOs; 

namespace Hanodale.DataAccessLayer.Interfaces
{
    public interface IUserProfileService
    {
        #region UserProfile

        UserProfileDetails GetUserProfileBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search);

        UserProfileDetails GetUserProfile(int currentUserId, int userId, int startIndex, int pageSize);

        UserProfiles CreateUserProfile(int currentUserId, UserProfiles userProfileEn, string pageName);

        UserProfiles UpdateUserProfile(int currentUserId, UserProfiles userProfileEn, string pageName);

        bool DeleteUserProfile(int currentUserId, int userProfileId, string pageName);

        UserProfiles GetUserProfileById(int id);

        bool IsUserProfileExists(UserProfiles userProfile);

        //List<UserProfiles> GetUserProfileList();
        #endregion
    }
}
