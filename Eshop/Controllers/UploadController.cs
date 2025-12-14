using System.Threading.Tasks;
using CoreLayer.Services;
using Common.Application.FileUtil;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Controllers
{
    public class UploadController : Controller
    {
        private readonly IAppContext _appContext;

        public UploadController(IAppContext appContext)
        {
            _appContext = appContext;
        }
        #region Article

        [Authorize]
        [Route("/Upload/Article")]
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile upload)
        {
            if (upload == null)
            {
                return null;
            }
            var fileName = await SaveFileInServer.SaveFile(upload, Directories.ProductContent);

            var url = $"{Directories.ArticleContent.Replace("wwwroot", "")}/{fileName}";
            return Json(new { uploaded = true, url });
        }
        [Authorize]
        [Route("/Upload/Product")]
        [HttpPost]
        public async Task<IActionResult> UploadProductImage(IFormFile upload)
        {
            if (upload == null)
            {
                return null;
            }
            var fileName = await SaveFileInServer.SaveFile(upload, Directories.ArticleContent);

            var url = $"{Directories.ArticleContent.Replace("wwwroot", "")}/{fileName}";
            return Json(new { uploaded = true, url });
        }
        #endregion

        #region NewsLetters

        [Authorize]
        [Route("/Upload/newsletter")]
        [HttpPost]
        public async Task<IActionResult> UploadNewsletterImage(IFormFile upload)
        {
            if (upload == null)
            {
                return null;
            }
            var fileName = await SaveFileInServer.SaveFile(upload, Directories.NewsLetterContent);

            var url = $"{_appContext.SiteBaseUrl}{Directories.NewsLetterContent.Replace("wwwroot", "")}/{fileName}";
            return Json(new { uploaded = true, url });
        }

        #endregion
    }
}
