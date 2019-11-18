using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.ViewModels
{
    /// <summary>
    /// 用户申请的应用
    /// </summary>
    public class ApplicationViewModel
    {
        /// <summary>
        /// Clients primarykey
        /// </summary>
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecrets { get; set; }

        public bool Enable { get; set; }

        /// <summary>
        /// 用户申请的权限
        /// </summary>
        public List<ClientScope> AllowedScopes { get; set; }

        /// <summary>
        /// 官方提供的权限
        /// </summary>
        public List<ScopeViewModel> scopeViewModels { get; set; }

        /// <summary>
        /// 应用类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 授权类型
        /// </summary>
        public string GrantType { get; set; }

        public DateTime Created { get; set; }
    }
}
