#pragma checksum "C:\study\OAuth\Info.Web\Views\About\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d9002fa2255d95b516e3f723ffda9ba59fc17f91"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_About_Index), @"mvc.1.0.view", @"/Views/About/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\study\OAuth\Info.Web\Views\_ViewImports.cshtml"
using Info;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\study\OAuth\Info.Web\Views\_ViewImports.cshtml"
using Info.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
using Microsoft.AspNetCore.Authentication;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
using Newtonsoft.Json.Linq;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
using System.Text;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
using IdentityModel;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d9002fa2255d95b516e3f723ffda9ba59fc17f91", @"/Views/About/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9791c88f3159db757a03c8890d3e7bcd156fc50f", @"/Views/_ViewImports.cshtml")]
    public class Views_About_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("action", new global::Microsoft.AspNetCore.Html.HtmlString("/Account/Logout"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "get", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("display: initial;"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 5 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
  
    Layout = null;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<!DOCTYPE html>\r\n\r\n<html>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d9002fa2255d95b516e3f723ffda9ba59fc17f915218", async() => {
                WriteLiteral("\r\n    <meta name=\"viewport\" content=\"width=device-width\" />\r\n    <title>UserInfo</title>\r\n    <style>\r\n        .warp {\r\n            width: 100%;\r\n            height: 100%;\r\n            word-wrap: break-word;\r\n        }\r\n    </style>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d9002fa2255d95b516e3f723ffda9ba59fc17f916438", async() => {
                WriteLiteral("\r\n");
#nullable restore
#line 24 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
     if (User.Identity.IsAuthenticated)
    {

#line default
#line hidden
#nullable disable
                WriteLiteral("        <br />\r\n        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d9002fa2255d95b516e3f723ffda9ba59fc17f916935", async() => {
                    WriteLiteral("\r\n            <button id=\"btn\">退出登陆</button>\r\n        ");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_1.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n        <a href=\"/\" style=\"text-decoration:none;border: 1px solid gray;padding: 10px;/* line-height: 13px; */display: inline-block;width: 42px;line-height: 5px;text-align: center;border-radius: 10px;color: black;\">首页</a>\r\n        <br />\r\n        ");
                WriteLiteral("个人信息：\r\n<span>用户名：</span>");
#nullable restore
#line 33 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
            Write(User.Identity.Name);

#line default
#line hidden
#nullable disable
                WriteLiteral("<br />\r\n");
#nullable restore
#line 36 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
}

#line default
#line hidden
#nullable disable
                WriteLiteral("    <br />\r\n");
#nullable restore
#line 38 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
     if (User.Identity.IsAuthenticated)
    {

#line default
#line hidden
#nullable disable
                WriteLiteral("        <span style=\"color:red\">id_token:</span><br />\r\n        <div class=\"warp\">\r\n            ");
#nullable restore
#line 42 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
       Write(await ViewContext.HttpContext.GetTokenAsync("id_token"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n        </div>\r\n        <br />\r\n        <span style=\"color:red\">access_token:</span>\r\n        <br />\r\n        <div class=\"warp\">\r\n            ");
#nullable restore
#line 48 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
       Write(await ViewContext.HttpContext.GetTokenAsync("access_token"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n        </div>\r\n        <br />\r\n        <span style=\"color:red\">refresh_token:</span>\r\n        <br />\r\n        <div class=\"warp\">\r\n            ");
#nullable restore
#line 54 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
       Write(await ViewContext.HttpContext.GetTokenAsync("refresh_token"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n        </div>\r\n        <br />\r\n        <span style=\"color:red\">Claims信息:</span><br />\r\n");
#nullable restore
#line 58 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
         foreach (var item in User.Claims)
        {

#line default
#line hidden
#nullable disable
                WriteLiteral("            <span>");
#nullable restore
#line 60 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
             Write(item.Type);

#line default
#line hidden
#nullable disable
                WriteLiteral(":</span>\r\n            <span>");
#nullable restore
#line 61 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
             Write(item.Value);

#line default
#line hidden
#nullable disable
                WriteLiteral(":</span><br />\r\n");
#nullable restore
#line 62 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 62 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
         
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 64 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
     if (User.Identity.IsAuthenticated && ViewContext.HttpContext.GetTokenAsync("access_token").Result.Contains("."))
    {
        var parts = ViewContext.HttpContext.GetTokenAsync("access_token").Result.Split('.');
        var header = parts[0];
        var claims = parts[1];
        var json = JObject.Parse(Encoding.UTF8.GetString(Base64Url.Decode(claims)));

#line default
#line hidden
#nullable disable
                WriteLiteral("        <br />\r\n        <span style=\"color:red\">access_token DeCode信息:</span><br />\r\n        <span>用户名：");
#nullable restore
#line 72 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
             Write(json.GetValue("name"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</span>\r\n        <br />\r\n        <span>邮箱：");
#nullable restore
#line 74 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
            Write(json.GetValue("email"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</span>\r\n");
#nullable restore
#line 75 "C:\study\OAuth\Info.Web\Views\About\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</html>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
