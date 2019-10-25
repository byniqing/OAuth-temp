using System.ComponentModel.DataAnnotations;

namespace IdentityServer.ViewModels
{
    public class LoginInputModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage ="邮箱不能为空")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?){1}$",ErrorMessage ="邮箱格式不正确")]
        public string Email { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }
        /// <summary>
        /// 是否记住我
        /// </summary>
        public bool RememberLogin { get; set; }
        /// <summary>
        /// 登录后的返回路径
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 用户登录方式  login/cancel
        /// </summary>
        public string button { get; set; }
    }
}
