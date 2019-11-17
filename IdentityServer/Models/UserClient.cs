using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    /// <summary>
    /// 用户开通的应用授权
    /// </summary>
    public class UserClient
    {
        public int Id { get; set; }

        /// <summary>
        /// 关联客户端的id
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 应用类型。web,aid(android的简称)
        /// </summary>
        public string Type { get; set; }
       
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }
    }
}
