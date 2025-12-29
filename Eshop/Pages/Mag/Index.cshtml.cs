using CoreLayer.DTOs.Mag;
using CoreLayer.Services.Articles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Eshop.Pages.Mag;

public class IndexModel : PageModel
{
    private readonly IArticleServices _articleServices;

    public IndexModel(IArticleServices articleServices)
    {
        _articleServices = articleServices;
    }

    public ArticleCategory ArticleCategory { get; set; }

    public async Task OnGet(int pageId = 1,string category = "",string search = null)
    {
        ArticleCategory = await _articleServices.GetArticleForCategory(pageId, 12, search, category);
    }
}
