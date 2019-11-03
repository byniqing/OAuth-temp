using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Resource.Models
{
    public class Permissions
    {
        public const string Role = "Role";
        public const string RoleCreate = "Role.Create";
        public const string RoleRead = "Role.Read";
        public const string RoleUpdate = "Role.Update";
        public const string RoleDelete = "Role.Delete";

        public const string User = "User";
        public const string UserCreate = "Create";
        public const string UserRead = "Read";
        public const string UserUpdate = "Update";
        public const string UserDelete = "Delete";
    }
}
