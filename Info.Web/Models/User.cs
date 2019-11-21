using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Info.Models
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty("name")]
        public string UserName { get; set; }

        /// <summary>
        /// 如果是第三方登录，则存储第三方用户的id作为唯一标识
        /// 不能用email或者phone，因为对方有可能更换
        /// </summary>
        [JsonProperty("sub")]
        public int BindId { get; set; }

        public string Address { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        //[JsonProperty("email")]  // 用JsonProperty特性
        public string Email { get; set; }

        /// <summary>
        /// 邮件是否已经验证
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 手机是否已经验证
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }


        /// <summary>
        /// 注册来源。
        /// 比如第三方
        /// 本地
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }
    }
}
