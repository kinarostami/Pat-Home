using Common.Application.UserUtil;
using CoreLayer.DTOs.Admin.Articles;
using CoreLayer.Services.Articles;
using CoreLayer.Utilities;
using DomainLayer.Models.Articles;
using DomainLayer.Models.Roles;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Eshop.Areas.Admin.Pages.Articles;
//[PermissionChecker(Permissions.مدیریت_مقالات)]

//[ValidateAntiForgeryToken]
public class AddModel : PageUtil
{
    private readonly IArticleServices _articleServices;

    public AddModel(IArticleServices articleServices)
    {
        _articleServices = articleServices;
    }

    [BindProperty]
    public AddArticleViewModel ArticleModel { get; set; }
    public List<ArticleGroup> MainGroups { get; set; }
    public List<ArticleGroup> SubGroups { get; set; }

    public void OnGet()
    {
        MainGroups = _articleServices.GetArticleGroups().Where(x => x.ParentId == null).ToList();
    }

    public async Task<IActionResult> OnPost()
    {
        if (await _articleServices.IsSubjectExist(ArticleModel.Title))
        {
            ModelState.AddModelError("ArticleModel.Title", "عنوان انتخاب شده قبلا انتخاب شده");
            InitDate();
            return Page();
        }
        if (await _articleServices.IsUrlIsExist(ArticleModel.Url))
        {
            ModelState.AddModelError("ArticleModel.Url", "آدرس وارد شده تکراری است");
            InitDate();
            return Page();
        }
        if (ArticleModel.ImageSelector == null)
        {
            TempData["Error"] = "true";
            InitDate();
            ModelState.AddModelError("ArticleModel.ImageSelector", "عکس مقاله را وارد کنید");
            return Page();
        }
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "true";
            InitDate();
            return Page();
        }

        ArticleModel.UserId = User.GetUserId();
        var res = await _articleServices.AddArticle(ArticleModel);
        if (res)
        {
            TempData["Success"] = "true";
            return RedirectToPage("Index");
        }

        InitDate();
        TempData["Error"] = "true";
        return Page();
    }

    public IActionResult OnGetGroupsByParent(long parentId)
    {
        var groups = _articleServices.GetArticleGroups().Where(g => g.ParentId == parentId);
        var options = "<option value='0'>انتخاب کنید</option>";

        foreach (var group in groups)
        {
            options += @$"<option value={group.Id}>{group.GroupTitle}</option>";
        }
        return Content(options);
    }
    private void InitDate()
    {
        var groups = _articleServices.GetArticleGroups();
        MainGroups = groups.Where(g => g.ParentId == null).ToList();
        SubGroups = groups.Where(g => g.ParentId == ArticleModel.GroupId).ToList();
    }
}
