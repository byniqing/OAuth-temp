using System.ComponentModel.DataAnnotations;

namespace Info.ViewModels
{
    public class LoginInputModel : AccountInputModel
    {
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
