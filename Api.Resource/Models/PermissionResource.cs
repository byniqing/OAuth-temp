using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Resource.Models
{
    /// <summary>
    /// 权限项拥有的资源，可以操作的资源
    /// </summary>
    public class PermissionResource
    {
        public int Id { get; set; }

        /// <summary>
        /// 关联RolePermission 表id
        /// </summary>
        public int RolePermissionId { get; set; }

        /// <summary>
        /// 请求Url
        /// </summary>
        public string Url { get; set; }

    }
}
