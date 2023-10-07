using CA.Domain.Base;
using System.ComponentModel;
using System.Reflection;

namespace CA.Domain.Constants.Permission
{
    public static class Permissions
    {
        public static class AllPermision
        {
            private static List<Claims> _permisionList;
            public static List<Claims> GetAllPermision()
            {
                if (_permisionList == null)
                {
                    var list = new List<Claims>();
                    var theList = Assembly.GetExecutingAssembly().GetTypes()
                              .Where(t => t.Namespace == "CA.Domain.Constants.Permission")
                              .ToList();
                    foreach (var type in theList)
                    {
                        foreach (var item in type.GetFields())
                        {
                            var obj = item.GetValue(item);
                            if (obj is string)
                            {
                                list.Add(new Claims { Id = obj as string, ClaimValue = obj as string , Group = type.Name});
                            }
                        }

                    }
                    list.RemoveAt(0);
                    _permisionList = list;
                }
                
                return _permisionList;
            }
        }

        [DisplayName("Test")]
        [Description("Test Permissions")]
        public static class TestPermissions
        {
            public const string View = "Permissions.Test.View";
            public const string Create = "Permissions.Test.Create";
            public const string Edit = "Permissions.Test.Edit";
            public const string Delete = "Permissions.Test.Delete";
        }


        [DisplayName("Users")]
        [Description("Users Permissions")]
        public static class UsersPermissions
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
        }

        [DisplayName("Roles")]
        [Description("Roles Permissions")]
        public static class RolesPermissions
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
        }


        [DisplayName("UsersRole")]
        [Description("UsersRole Permissions")]
        public static class UsersRolePermissions
        {
            public const string View = "Permissions.UsersRole.View";
            public const string Create = "Permissions.UsersRole.Create";
            public const string Edit = "Permissions.UsersRole.Edit";
            public const string Delete = "Permissions.UsersRole.Delete";
        }

        [DisplayName("Role Claims")]
        [Description("Role Claims Permissions")]
        public static class RoleClaimsPermissions
        {
            public const string View = "Permissions.RoleClaims.View";
            public const string Create = "Permissions.RoleClaims.Create";
            public const string Edit = "Permissions.RoleClaims.Edit";
            public const string Delete = "Permissions.RoleClaims.Delete";
        }

        [DisplayName("TValue")]
        [Description("TValue Permissions")]
        public static class TValuePermissions
        {
            public const string View = "Permissions.TValue.View";
            public const string Create = "Permissions.TValue.Create";
            public const string Edit = "Permissions.TValue.Edit";
            public const string Delete = "Permissions.TValue.Delete";
        }


    }
}
