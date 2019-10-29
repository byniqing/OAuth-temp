using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    /// <summary>
    /// 定义自己的实体
    /// </summary>
    public class ApplicationUser : IdentityUser<int> //默认主键id是string的，所以自定义实体
    {
        //可以新增自己的一些属性


        //[Required]
        //public string Name { get; set; }
        //[Required]
        //public string Street { get; set; }
        //[Required]
        //public string City { get; set; }
        //[Required]
        //public string State { get; set; }
        //[Required]
        //public string Country { get; set; }
        //[Required]
        //public string ZipCode { get; set; }
        //[Required]
        //public string LastName { get; set; }
    }
}
