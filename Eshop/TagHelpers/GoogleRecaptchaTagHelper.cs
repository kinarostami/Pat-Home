using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;

namespace Eshop.TagHelpers
{
    [HtmlTargetElement("google-recaptcha")]
    public class GoogleRecaptchaTagHelper : TagHelper
    {
        private readonly IConfiguration _configuration;

        public GoogleRecaptchaTagHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.AddClass("g-recaptcha", HtmlEncoder.Default);
            output.Attributes.Add("data-sitekey", _configuration.GetSection("Recaptcha")["SiteKey"]);
            base.Process(context, output);
        }
    }
}