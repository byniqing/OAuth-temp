using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Resource.Models
{
    /// <summary>
    /// 角色拥有的权限项
    /// </summary>
    public class RolePermission
    {
        public int Id { get; set; }

        /// <summary>
        /// 角色id
        /// 关联Role表id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 权限项名称 CRUD
        /// Create
        /// Read
        /// Update
        /// Delete
        /// </summary>
        public string PermissionName { get; set; }

        /// <summary>
        /// 当前权限可以操作的数据,拥有的资源
        /// </summary>
        public List<PermissionResource> permissionResources { get; set; }
    }
}
