﻿using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Authertication
{
    /*
     * form-data 方式
     * 
     grant_type:sms_auth_code
client_id:android
client_secret:secret
scope:openid profile offline_access OtherInfo
phone:15919099881
auth_code:111
         */
    public class SmsAuthCodeValidator: IExtensionGrantValidator
    {
        //private IUserService _userService;
        //private IAuthCodeService _authCodeService;

        //public SmsAuthCodeValidator(IUserService userService, IAuthCodeService authCodeService)
        //{
        //    _userService = userService;
        //    _authCodeService = authCodeService;
        //}
        public string GrantType => "sms_auth_code";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var phone = context.Request.Raw["phone"];
            var code = context.Request.Raw["auth_code"];
            var error = new GrantValidationResult(TokenRequestErrors.InvalidGrant);

            ////验证验证码
            //if (!_authCodeService.Validate(phone, code))
            //{
            //    context.Result = error;
            //    return;
            //}

            ////用户注册
            //var userId = await _userService.CheckOrCreate(phone);
            //if (userId <= 0)
            //{
            //    context.Result = error;
            //    return;
            //}

            //context.Result = new GrantValidationResult(userId.ToString(), GrantType);
            context.Result = new GrantValidationResult("1", GrantType);
        }
    }
}
