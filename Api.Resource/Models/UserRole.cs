using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Resource.Models
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class UserRole
    {
        public int Id { get; set; }

        /// <summary>
        /// 角色id
        /// 关联Role表id
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 具体权限
        /// </summary>
        public Role Role { get; set; }
        /// <summary>
        /// 角色拥有的权限项
        /// </summary>

        public List<RolePermission> Permissions { get; set; }
    }
}
