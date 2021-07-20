using System;
using System.Web.Security;

using Epam.Internet_shop.Logger.Contracts;
using Epam.Internet_shop.BLL.Contracts;
using Epam.Internet_shop.Common;

namespace Epam.Internet_shop.PL.Web.App_Code
{
    public class Provider : RoleProvider
    {
        private readonly ILogger _logger;

        private readonly IUserBll _userBll;

        public Provider()
        {
            _logger = DependencyResolver.Logger;

            _userBll = DependencyResolver.UserBll;
        }

        public override string[] GetRolesForUser(string login)
        {
            _logger.Info($"PL.{nameof(Provider)}.{nameof(GetRolesForUser)}: Get Role for user \"{login}\"");

            return new string[] { _userBll.GetUserOrNull(login).Role.Name };
        }

        public override bool IsUserInRole(string login, string roleName)
        {
            _logger.Info($"PL.{nameof(Provider)}.{nameof(IsUserInRole)}: Role check for user \"{login}\"");

            if (_userBll.GetUserOrNull(login).Role.Name == roleName)
            {
                _logger.Info($"PL.{nameof(Provider)}.{nameof(IsUserInRole)}: The role is valid");

                return true;
            }
            else
            {
                _logger.Info($"PL.{nameof(Provider)}.{nameof(IsUserInRole)}: The role isn't valid");

                return false;
            }
        }

        #region Not implemented

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}