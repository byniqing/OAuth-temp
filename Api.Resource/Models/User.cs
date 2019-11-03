using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Resource.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public DateTime Birthday { get; set; }

        /// <summary>
        /// 用户所属角色
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// 角色对应的权限
        /// </summary>

        public List<UserPermission> Permissions { get; set; }
    }
}
