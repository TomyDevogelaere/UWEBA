using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Memory;
using ScriptMarker.Utils;
using System.Text.Encodings.Web;

namespace ASPNET6.ScriptMarker.TagHelpers;
[HtmlTargetElement(tag: "img",
   Attributes = $"{RenderBase64AttributeName},{SrcAttributeName}",
   TagStructure = TagStructure.WithoutEndTag)]
public class ImageBase64TagHelper: UrlResolutionTagHelper
{
    private const string RenderBase64AttributeName = "asp-render-base64";

    private const string SrcAttributeName = "src";

    private IWebHostEnvironment HostEnvironment { get; }
    private IMemoryCache Cache { get; }

    public ImageBase64TagHelper(IWebHostEnvironment hostingEnvironment,
                            IMemoryCache cache,
                            HtmlEncoder htmlEncoder,
                            IUrlHelperFactory urlHelperFactory)
  : base(urlHelperFactory, htmlEncoder)
    {
        HostEnvironment = hostingEnvironment;
        Cache = cache;
    }


    [HtmlAttributeName(SrcAttributeName)]
    public string? Src { get; set; }

    [HtmlAttributeName(RenderBase64AttributeName)]
    public bool RenderBase64 { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.CopyHtmlAttribute(SrcAttributeName, context);

        if (RenderBase64)
        {
            this.base64Provider ??= EnsureBase64ContentProvider();
            Src = output.Attributes[SrcAttributeName].Value as string;
            if (this.base64Provider is not null && Src is not null)
            {
                string? base64 = this.base64Provider.RetrieveBase64Data(Src);
                output.Attributes.SetAttribute(SrcAttributeName, base64);
            }
        }
    }

    private FileBase64ContentProvider? base64Provider;

    private FileBase64ContentProvider EnsureBase64ContentProvider()
      => new FileBase64ContentProvider(
        fileProvider: HostEnvironment.WebRootFileProvider,
        cache: Cache,
        requestPathBase: ViewContext.HttpContext.Request.PathBase);
}
