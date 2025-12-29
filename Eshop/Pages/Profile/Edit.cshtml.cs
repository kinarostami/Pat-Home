using CoreLayer.DTOs.Profile;
using CoreLayer.Services.Users;
using Common.Application.DateUtil;
using Common.Application.SecurityUtil;
using Common.Application.UserUtil;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Pages.Profile
{
    //[PermissionChecker(Permissions.ویرایش_اطلاعات)]
    [ValidateAntiForgeryToken]
    public class EditModel : PageUtil
    {
        private readonly IUserService _userService;

        public EditModel(IUserService userService)
        {
            _userService = userService;
        }
        [BindProperty]
        public EditProfileDto EditDto { get; set; }
        public async Task<IActionResult> OnGet(string completed)
        {
            if (!string.IsNullOrEmpty(completed))
            {
                TempData["CompleteInfo"] = true;
            }
            await ConvertUserToEditDto();
            return Page();
        }

        public async Task<IActionResult> OnPost(string birth)
        {
            EditDto.UserId = User.GetUserId();
            EditDto.BirthDate = birth.ToMiladi();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return await TryCatch(async () =>
            {
                if (EditDto.Email != EditDto.OldEmail)
                {
                    if (await _userService.IsEmailExist(EditDto.Email))
                    {
                        throw new Exception("ایمیل وارد شده تکراری است");
                    }
                }
                if (EditDto.ImageFile != null && !EditDto.ImageFile.IsImage())
                {
                    throw new Exception("عکس وارد شده نامعتبر است");
                }
                var user = await _userService.EditUser(EditDto);

                #region SetUserInCookie

                CookieJobs.SetUserCookie(HttpContext, user);

                #endregion
            }, successReturn: "/Profile");
        }

        #region Utils

        private async Task ConvertUserToEditDto()
        {
            var user = await _userService.GetSingleUser(User.GetUserId());

            EditDto = new EditProfileDto()
            {
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                Family = user.Family,
                Name = user.Name,
                Email = user.Email,
                NationalCode = user.NationalCode,
                ImageName = user.ImageName,
                OldEmail = user.Email,
                BirthDate = user.BirthDate,
                IsCompleteUProfile = user.IsCompleteProfile
            };
        }

        #endregion
    }
}
