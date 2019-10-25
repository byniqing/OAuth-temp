using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Info.ViewModels
{
    public class ExternalProvider
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 认证方案
        /// </summary>
        public string AuthenticationScheme { get; set; }
    }
}
