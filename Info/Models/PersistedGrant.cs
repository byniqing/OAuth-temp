using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Info.Models
{
    /// <summary>
    /// 授权信息
    /// </summary>
    public class PersistedGrant
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string Type { get; set; }
        public string Token { get; set; }
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Expiration { get; set; }

    }
}
