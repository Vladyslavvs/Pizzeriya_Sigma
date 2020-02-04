using Pizzeriya.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Pizzeriya.Providers
{
    public class MyRoleProvider : RoleProvider
    {
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

        public override string[] GetRolesForUser(string username)
        {
            int Id = int.Parse(username);
            string[] roles = new string[] { };
            using (DBContext  db=new DBContext())
            {
                User user = db.Users.Where(u => u.UserId == Id).FirstOrDefault();
                if (user != null)
                {
                    Role UserRole = db.Roles.Where(r => r.Id == user.RoleId).FirstOrDefault();
                    if (UserRole != null)
                    {
                        roles = new string[] { UserRole.Name };
                    }
                }
            }
            return roles;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            int Id = int.Parse(username);
            bool result = false;
            using (DBContext db = new DBContext())
            {
                User user = db.Users.Where(u => u.UserId == Id).FirstOrDefault();
                if (user != null)
                {
                    Role UserRole = db.Roles.Where(r => r.Id == user.RoleId).FirstOrDefault();
                    if (UserRole != null&&UserRole.Name==roleName)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}