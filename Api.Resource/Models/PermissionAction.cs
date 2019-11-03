using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Resource.Models
{
    /// <summary>
    /// 权限对应的具体动作（路径）
    /// </summary>
    public class PermissionAction
    {
        public int Id { get; set; }
        /// <summary>
        /// 请求Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PermissionId { get; set; }
      
    }
}
