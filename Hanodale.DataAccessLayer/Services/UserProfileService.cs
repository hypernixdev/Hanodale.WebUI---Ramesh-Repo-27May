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
using Hanodale.DataAccessLayer.Interfaces;

namespace Hanodale.DataAccessLayer.Services
{
    public class UserProfileService : IUserProfileService
    {
        #region UserProfile

        /// <summary>
        /// This method is to get the stock details with search
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>User list</returns> 

        public UserProfileDetails GetUserProfileBySearch(int currentUserId, int userId, int startIndex, int pageSize, string search)
        {
            
            return null;
        }

        /// <summary>
        /// This method is to get the UserProfile details
        /// </summary>
        /// <param name="startIndex">start page</param>
        /// <param name="pageSize">page size eg: 10 </param>
        /// <returns>User list</returns>  
        public UserProfileDetails GetUserProfile(int currentUserId, int userId, int startIndex, int pageSize)
        {
           
            return null;
        }

        /// <summary>
        /// This method is to save the UserProfile details
        /// </summary> 
        public UserProfiles CreateUserProfile(int currentUserId, UserProfiles userProfileEn, string pageName)
        {
       
            return null;
        }

        /// <summary>
        /// This method is to update the UserProfile details
        /// </summary> 
        public UserProfiles UpdateUserProfile(int currentUserId, UserProfiles userProfileEn, string pageName)
        {
            
            return null;
        }

        /// <summary>
        /// This method is to delete the UserProfile details
        /// </summary>
        /// <param name="UserProfileId">UserProfile id</param>  
        public bool DeleteUserProfile(int currentUserId, int userProfileId, string pageName)
        {
            bool isDeleted = false;
            
            return isDeleted;
        }

        /// <summary>
        /// This method is to get the UserProfile by UserProfile id
        /// </summary>
        /// <param name="roleId">UserProfile Id</param>
        /// <returns>UserProfiles details</returns>
        public UserProfiles GetUserProfileById(int id)
        {
            
            return null;
        }

        ///// <summary>
        ///// This method is to check the UserProfile exists or not.
        ///// </summary>
        ///// <param name="UserProfileName">UserProfile Name</param>  
        public bool IsUserProfileExists(UserProfiles userProfile)
        {
            bool isExists = false;
            
            return isExists;
        }

        #endregion
    }
}
