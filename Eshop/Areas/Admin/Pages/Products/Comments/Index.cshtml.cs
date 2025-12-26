using System.Threading.Tasks;
using CoreLayer.DTOs.Shop;
using CoreLayer.Services.Products;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Areas.Admin.Pages.Products.Comments
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _product;

        public IndexModel(IProductService product)
        {
            _product = product;
        }
        public ProductCommentsViewModel Comments { get; set; }
        public async Task OnGet(int pageId = 1, string startDate = "", string endDate = "")
        {
            Comments = await _product.GetCommentByFilter(pageId, 20, startDate, endDate);
        }
    }
}
