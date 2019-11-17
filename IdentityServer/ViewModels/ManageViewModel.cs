using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.ViewModels
{
    public class ManageViewModel
    {
        /// <summary>
        /// 身份资源
        /// </summary>
        public IEnumerable<ScopeViewModel> IdentityScopes { get; set; }
        /// <summary>
        /// Api资源
        /// </summary>
        public IEnumerable<ScopeViewModel> ResourceScopes { get; set; }
    }
}
