#pragma checksum "D:\School Work\Yr 3 Season 2\Phase 2\Advanced Web Programming\Midterm Exam\SIS\Views\Shared\_ModalFooter.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1d6796be9b12fcf845ce1ae393511afa1df48f5d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__ModalFooter), @"mvc.1.0.view", @"/Views/Shared/_ModalFooter.cshtml")]
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
#line 1 "D:\School Work\Yr 3 Season 2\Phase 2\Advanced Web Programming\Midterm Exam\SIS\Views\_ViewImports.cshtml"
using SIS;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\School Work\Yr 3 Season 2\Phase 2\Advanced Web Programming\Midterm Exam\SIS\Views\_ViewImports.cshtml"
using SIS.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1d6796be9b12fcf845ce1ae393511afa1df48f5d", @"/Views/Shared/_ModalFooter.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"896f2525ffbc174c29328574f62967999aa136c4", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__ModalFooter : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ModalFooter>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<div class=\"modal-footer\">\r\n    <button data-dismiss=\"modal\"");
            BeginWriteAttribute("id", " id=\"", 82, "\"", 108, 1);
#nullable restore
#line 4 "D:\School Work\Yr 3 Season 2\Phase 2\Advanced Web Programming\Midterm Exam\SIS\Views\Shared\_ModalFooter.cshtml"
WriteAttributeValue("", 87, Model.CancelButtonID, 87, 21, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"btn btn-default\" type=\"button\">");
#nullable restore
#line 4 "D:\School Work\Yr 3 Season 2\Phase 2\Advanced Web Programming\Midterm Exam\SIS\Views\Shared\_ModalFooter.cshtml"
                                                                                             Write(Model.CancelButtonText);

#line default
#line hidden
#nullable disable
            WriteLiteral("</button>\r\n");
#nullable restore
#line 5 "D:\School Work\Yr 3 Season 2\Phase 2\Advanced Web Programming\Midterm Exam\SIS\Views\Shared\_ModalFooter.cshtml"
     if (!Model.OnlyCancelButton)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <button class=\"btn btn-success\"");
            BeginWriteAttribute("id", " id=\"", 263, "\"", 289, 1);
#nullable restore
#line 7 "D:\School Work\Yr 3 Season 2\Phase 2\Advanced Web Programming\Midterm Exam\SIS\Views\Shared\_ModalFooter.cshtml"
WriteAttributeValue("", 268, Model.SubmitButtonID, 268, 21, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" type=\"submit\">            \r\n               ");
#nullable restore
#line 8 "D:\School Work\Yr 3 Season 2\Phase 2\Advanced Web Programming\Midterm Exam\SIS\Views\Shared\_ModalFooter.cshtml"
          Write(Model.SubmitButtonText);

#line default
#line hidden
#nullable disable
            WriteLiteral("</button>\r\n");
#nullable restore
#line 9 "D:\School Work\Yr 3 Season 2\Phase 2\Advanced Web Programming\Midterm Exam\SIS\Views\Shared\_ModalFooter.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ModalFooter> Html { get; private set; }
    }
}
#pragma warning restore 1591
