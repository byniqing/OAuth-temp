using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Resource.Models
{
    public class UserPermission
    {
        public int Id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 权限名称
        /// CRUD
        /// Create
        /// Read
        /// Update
        /// Delete
        /// </summary>
        public string PermissionName { get; set; }

        /// <summary>
        /// 当前权限可以操作的数据
        /// </summary>
        public List<PermissionAction> permissionAction { get; set; }
    }
}
