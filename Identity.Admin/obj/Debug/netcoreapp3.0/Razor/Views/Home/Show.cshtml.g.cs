#pragma checksum "F:\project\OAuth-Ids4\Identity.Admin\Views\Home\Show.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "92eff1f70316462c0b3c07e4b0104ad867a11a61"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Show), @"mvc.1.0.view", @"/Views/Home/Show.cshtml")]
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
#line 1 "F:\project\OAuth-Ids4\Identity.Admin\Views\_ViewImports.cshtml"
using Identity.Admin;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "F:\project\OAuth-Ids4\Identity.Admin\Views\_ViewImports.cshtml"
using Identity.Admin.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"92eff1f70316462c0b3c07e4b0104ad867a11a61", @"/Views/Home/Show.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b370ab1e12cc4c4ecd12cc6bcd616ca85d6f3b6d", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Show : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", new global::Microsoft.AspNetCore.Html.HtmlString("text/css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/css/demo.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/jquery-1.10.2.min.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/jquery.qrcode.min.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("by"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("overflow-y: hidden"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "F:\project\OAuth-Ids4\Identity.Admin\Views\Home\Show.cshtml"
  
    Layout = null;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<!DOCTYPE html>\r\n<html>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "92eff1f70316462c0b3c07e4b0104ad867a11a615983", async() => {
                WriteLiteral("\r\n    <meta charset=\"utf-8\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n\r\n    <title>Vali Admin</title>\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "92eff1f70316462c0b3c07e4b0104ad867a11a616459", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "92eff1f70316462c0b3c07e4b0104ad867a11a617725", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "92eff1f70316462c0b3c07e4b0104ad867a11a618824", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
    <script type=""text/javascript"">
        var dqfl = ""UI"";
        var theme_list_open = false;

        $(document).ready(function () {
            function fixHeight() {
                var headerHeight = $(""#switcher"").height();
                $(""#iframe"").attr(""height"", $(window).height() - 54 + ""px"");
            }
            $(window).resize(function () {
                fixHeight();
            }).resize();

            $('.icon-monitor').addClass('active');

            $("".icon-mobile-3"").click(function () {
                $(""#by"").css(""overflow-y"", ""auto"");
                $('#iframe-wrap').removeClass().addClass('mobile-width-3');
                $('.icon-tablet,.icon-mobile-1,.icon-monitor,.icon-mobile-2,.icon-mobile-3').removeClass('active');
                $(this).addClass('active');
                return false;
            });

            $("".icon-mobile-2"").click(function () {
                $(""#by"").css(""overflow-y"", ""auto"");
                $('#iframe-wrap");
                WriteLiteral(@"').removeClass().addClass('mobile-width-2');
                $('.icon-tablet,.icon-mobile-1,.icon-monitor,.icon-mobile-2,.icon-mobile-3').removeClass('active');
                $(this).addClass('active');
                return false;
            });


            $("".icon-mobile-1"").click(function () {
                $(""#by"").css(""overflow-y"", ""auto"");
                $('#iframe-wrap').removeClass().addClass('mobile-width');
                $('.icon-tablet,.icon-mobile,.icon-monitor,.icon-mobile-2,.icon-mobile-3').removeClass('active');
                $(this).addClass('active');
                return false;
            });

            $("".icon-tablet"").click(function () {
                $(""#by"").css(""overflow-y"", ""auto"");
                $('#iframe-wrap').removeClass().addClass('tablet-width');
                $('.icon-tablet,.icon-mobile-1,.icon-monitor,.icon-mobile-2,.icon-mobile-3').removeClass('active');
                $(this).addClass('active');
                return false;
 ");
                WriteLiteral(@"           });

            $("".icon-monitor"").click(function () {
                $(""#by"").css(""overflow-y"", ""hidden"");
                $('#iframe-wrap').removeClass().addClass('full-width');
                $('.icon-tablet,.icon-mobile-1,.icon-monitor,.icon-mobile-2,.icon-mobile-3').removeClass('active');
                $(this).addClass('active');
                return false;
            });

            if (dqfl.indexOf(""APP模板"") != -1) {
                $(""#by"").css(""overflow-y"", ""auto"");
                $('#iframe-wrap').removeClass().addClass('mobile-width-2');
                $('.icon-tablet,.icon-mobile-1,.icon-monitor,.icon-mobile-2,.icon-mobile-3').removeClass('active');
                $(this).addClass('active');
                return false;
            }
        });
    </script>
    <script type=""text/javascript"">
        function Responsive($a) {
            if ($a == true) $(""#Device"").css(""opacity"", ""100"");
            if ($a == false) $(""#Device"").css(""opacity"", ""0"");");
                WriteLiteral(@"
            $('#iframe-wrap').removeClass().addClass('full-width');
            $('.icon-tablet,.icon-mobile-1,.icon-monitor,.icon-mobile-2,.icon-mobile-3').removeClass('active');
            $(this).addClass('active');
            return false;
        };
    </script>
");
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
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "92eff1f70316462c0b3c07e4b0104ad867a11a6114156", async() => {
                WriteLiteral(@"
    <div id=""switcher"">
        <div class=""center"">
            <ul>
                <li class=""logoTop"" id=""sj2""><a href=""/"">Bootstrap后台管理系统模版</a></li>
                <div id=""Device"">
                    <li class=""device-monitor"">
                        <a href=""javascript:"">
                            <div class=""icon-monitor"">
                            </div>
                        </a>
                    </li>
                    <li class=""device-mobile"">
                        <a href=""javascript:"">
                            <div class=""icon-tablet"">
                            </div>
                        </a>
                    </li>
                    <li class=""device-mobile"">
                        <a href=""javascript:"">
                            <div class=""icon-mobile-1"">
                            </div>
                        </a>
                    </li>
                    <li class=""device-mobile-2"">
                        <a href=""javascript");
                WriteLiteral(@":"">
                            <div class=""icon-mobile-2"">
                            </div>
                        </a>
                    </li>
                    <li class=""device-mobile-3"">
                        <a href=""javascript:"">
                            <div class=""icon-mobile-3"">
                            </div>
                        </a>
                    </li>
                </div>
                <li class=""top2"" id=""sj"">
                    <a href=""#"">手机二维码预览</a><div class=""vm"">
                        <div id=""output""></div>
                        <p style=""color: #808080"">扫一扫，直接在手机上打开</p>
                    </div>
                </li>
                <script>
                    jQuery('#output').qrcode({ width: 150, height: 150, text: window.location.href });
                </script>
                <li class=""remove_frame"" style=""display:block""><a href=""/"" title=""移除框架""></a></li>
            </ul>
        </div>
    </div>
    <div id=""iframe-wr");
                WriteLiteral("ap\">\r\n        <iframe id=\"iframe\" src=\"/\" frameborder=\"0\" width=\"100%\"></iframe>\r\n    </div>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</html>");
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
