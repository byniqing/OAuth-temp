using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Remarks
{
    public class Identity
    {
        // https://www.jianshu.com/p/ad20944d9446
        /*
         ASP.NET Core Identity 库表分析

        _EFMigrationsHistory 是 Ef的迁移历史表不必关注此表

AspNetUserClaims、AspNetRoleClaims是用户和角色的声明表， Identity 是基于声明的认证模式（Claims Based Authentication）的，Claim在其中扮演者很重要的角色，甚至角色（Role）都被转换成了Claim。
AspNetUsers、AspNetRoles和AspNetUserRoles存储用户和角色信息。
AspNetUserTokens、AspNetUserLogins存储的是用户使用的外部登陆提供商的信息和Token，外部登陆提供商指的是像微博、QQ、微信、Google、微软这类提供 oauth 或者 openid connect 登录的厂商。

接下来就要解释下最为重要的一张表AspNetUsers

        date存入的数据格式为：yyyy-mm-dd

      datetime存入的数据格式为：yyyy-mm-dd hh:mm:ss.fff(精确到1毫秒)

      datetime2(7)存入的数据格式为：yyyy-mm-dd hh:mm:ss.fffffff（精确到0.1微秒）

      datetimeoffset(7)存入的数据格式为：yyyy-mm-dd hh:mm:ss（精确到0.1为微秒）
————————————————
版权声明：本文为CSDN博主「HZT—James」的原创文章，遵循 CC 4.0 BY-SA 版权协议，转载请附上原文出处链接及本声明。
原文链接：https://blog.csdn.net/weixin_43267344/article/details/85386424

         */
    }

    public class AspNetUsers
    {
        /// <summary>
        /// 主键
        /// 默认是 nvarchar(450)  GUID
        /// 我自动修改成了int
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 规范后的username
        /// </summary>
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// 邮件是否已经确认
        /// </summary>
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }

        /// <summary>
        /// 同步标记，每当用户记录被更改时必须要更改此列的值，
        /// 事实上存储的是Guid，并且在创建用户模型的时候直接在属性上初始化随机值
        /// </summary>
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// 这个是用来记录用户尝试登陆却登陆失败的次数，我们可以通过这个来确定在什么时候需要锁定用户，
        /// </summary>
        public int AccessFailedCount { get; set; }

        /// <summary>
        /// 规范后的Email，其实就是大小了
        /// </summary>
        public string NormalizedEmail { get; set; }
    }
}
