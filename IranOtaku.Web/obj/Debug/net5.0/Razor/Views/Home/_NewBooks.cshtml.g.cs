#pragma checksum "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\Home\_NewBooks.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a878066e3f569ab38444a4f03330999ad6d2bad1"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home__NewBooks), @"mvc.1.0.view", @"/Views/Home/_NewBooks.cshtml")]
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
#line 1 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\_ViewImports.cshtml"
using IranOtaku.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\_ViewImports.cshtml"
using IranOtaku.Web.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\_ViewImports.cshtml"
using IranOtaku.Core.ViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\_ViewImports.cshtml"
using Microsoft.EntityFrameworkCore;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\_ViewImports.cshtml"
using IranOtaku.Core.UnitOfWork.Repositories;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\_ViewImports.cshtml"
using IranOtaku.Core.UnitOfWork.Services;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\_ViewImports.cshtml"
using IranOtaku.Data.Entities;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a878066e3f569ab38444a4f03330999ad6d2bad1", @"/Views/Home/_NewBooks.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a3a6df417ba8cc7517cf18e2d26fe36e970cc669", @"/Views/_ViewImports.cshtml")]
    public class Views_Home__NewBooks : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Detail", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Home", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\Home\_NewBooks.cshtml"
  
    var types = await _bookManager.GetNewBooksAsync();

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 7 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\Home\_NewBooks.cshtml"
 foreach (var type in types)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <section class=\"broadcastCon\">\r\n        <div class=\"broadcastTitle\">\r\n            <span>آخرین ");
#nullable restore
#line 11 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\Home\_NewBooks.cshtml"
                   Write(type.TypeName);

#line default
#line hidden
#nullable disable
            WriteLiteral(" ها</span>\r\n        </div>\r\n        <div class=\"wapper\">\r\n");
#nullable restore
#line 14 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\Home\_NewBooks.cshtml"
             foreach (var book in type.Books)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <article class=\"broadcastArticle\">\r\n                    <div class=\"broadcastArticleImg\">\r\n                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a878066e3f569ab38444a4f03330999ad6d2bad15974", async() => {
                WriteLiteral("\r\n                            <img");
                BeginWriteAttribute("src", " src=\"", 604, "\"", 658, 2);
                WriteAttributeValue("", 610, "https://dl.aosasori.com/BookImages/", 610, 35, true);
#nullable restore
#line 19 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\Home\_NewBooks.cshtml"
WriteAttributeValue("", 645, book.Image, 645, 13, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("alt", " alt=\"", 659, "\"", 679, 2);
                WriteAttributeValue("", 665, "عکس", 665, 3, true);
#nullable restore
#line 19 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\Home\_NewBooks.cshtml"
WriteAttributeValue(" ", 668, book.Name, 669, 10, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">\r\n                        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 18 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\Home\_NewBooks.cshtml"
                                                                       WriteLiteral(book.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                    </div>\r\n                    <div class=\"broadcastArticleContant\">\r\n                        <div class=\"broadcastArticleName\">\r\n                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a878066e3f569ab38444a4f03330999ad6d2bad19436", async() => {
#nullable restore
#line 24 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\Home\_NewBooks.cshtml"
                                                                                            Write(book.Name);

#line default
#line hidden
#nullable disable
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 24 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\Home\_NewBooks.cshtml"
                                                                           WriteLiteral(book.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                        </div>\r\n                        <div class=\"bACCon\">\r\n                            <ul class=\"ulEpCon\">\r\n                                <li class=\"Condition\">");
#nullable restore
#line 28 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\Home\_NewBooks.cshtml"
                                                 Write(book.Age);

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\r\n                                <li class=\"Season\">");
#nullable restore
#line 29 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\Home\_NewBooks.cshtml"
                                              Write(book.Translator);

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\r\n                                <li class=\"Episodes\">");
#nullable restore
#line 30 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\Home\_NewBooks.cshtml"
                                                Write(book.TranslatorTeam);

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\r\n                            </ul>\r\n                            <ul class=\"ulBBiuCon\">\r\n");
#nullable restore
#line 33 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\Home\_NewBooks.cshtml"
                                 foreach (var category in book.Categories.Take(3))
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <li>");
#nullable restore
#line 35 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\Home\_NewBooks.cshtml"
                                   Write(category.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\r\n");
#nullable restore
#line 36 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\Home\_NewBooks.cshtml"
                                }

#line default
#line hidden
#nullable disable
            WriteLiteral("                            </ul>\r\n                        </div>\r\n                    </div>\r\n                </article>\r\n");
#nullable restore
#line 41 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\Home\_NewBooks.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </div>\r\n    </section>\r\n");
#nullable restore
#line 45 "D:\Asp.netCore\IranOtaku\IranOtaku.Web\Views\Home\_NewBooks.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IBookManagement _bookManager { get; private set; }
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
