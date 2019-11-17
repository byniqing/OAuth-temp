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
        public string ClientName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecrets { get; set; }

        public bool Enable { get; set; }

        public DateTime Created { get; set; }
    }
}
