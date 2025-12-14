using System;
using System.Threading.Tasks;
using Common.Application;
using CoreLayer.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Eshop.Infrastructure
{
    public class PageUtil : PageModel
    {
        public async Task<IActionResult> TryCatch(Func<Task> func,
            string successReturn = null,
            string successMessage = null,
            string successTitle = null,
            string errorReturn = null,
            bool showAlert = true,
            string errorMessage = null)
        {
            try
            {
                await func();
                if (showAlert)
                {
                    TempData["Success"] = ResultModel.Success(successMessage, successTitle);
                }
                if (successReturn != null)
                {
                    return Redirect(successReturn);
                }
                return Page();
            }
            catch (Exception ex)
            {

                TempData["Error"] = ResultModel.Error(errorMessage ?? ex.Message);
                if (errorReturn != null)
                {
                    return Redirect(errorReturn);
                }

                return Page();
            }
        }
        public async Task<ContentResult> AjaxTryCatch(Func<Task> func,
            string successMessage = "",
            string successTitle = "",
            string successReturn = null,
            string errorReturn = null)
        {
            try
            {
                await func();
                string jsonString = ResultModel.Success(successMessage, successTitle);
                return Content(successReturn ?? jsonString);
            }
            catch (Exception ex)
            {
                string jsonString = ResultModel.Error(ex.Message);
                return Content(errorReturn ?? jsonString);
            }
        }
    }
    public class Result
    {
        public string Title { get; set; } = null;
        public string Message { get; set; }
        public ResultType Type { get; set; }
    }
    public static class ResultModel
    {
        public static string Success(string message, string title)
        {
            var resultModel = new Result()
            {
                Message = message,
                Title = title ?? "عملیات با موفقیت انجام شد",
                Type = ResultType.Success,
            };
            return JsonConvert.SerializeObject(resultModel);
        }

        public static string Error(string message)
        {
            var messageBody = "";
            if (message != null)
            {
                messageBody = message.IsUniCode() ? message : messageBody;
            }
            var resultModel = new Result()
            {
                Message = messageBody,
                Type = ResultType.Error,
            };
            return JsonConvert.SerializeObject(resultModel);
        }
    }

    public enum ResultType
    {
        Success,
        Error
    }
}