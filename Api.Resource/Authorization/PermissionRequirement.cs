using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Resource.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string name, string claimType, string path)
        {
            Name = name;
            ClaimType = claimType;
            Path = path;
        }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 认证授权类型
        /// internal get
        /// </summary>
        public string ClaimType { get; set; }

        /// <summary>
        /// 请求路径
        /// </summary>
        public string Path { get; set; } = "/Api/Login";
    }
}
