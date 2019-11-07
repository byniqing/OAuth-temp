using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    /// <summary>
    /// 用户授权第三方登录的信息
    /// </summary>
    public class ApplicationUseAuthorization
    {
        public int Id { get; set; }

        /// <summary>
        /// 关联客户端的id
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// 当用取消授权后，置为false(0)
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }
    }
}
