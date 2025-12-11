using Common.Application;
using Common.Application.FileUtil;
using Common.Application.SecurityUtil;
using CoreLayer.DTOs.Admin.Users;
using CoreLayer.DTOs.Auth;
using CoreLayer.DTOs.Profile;
using DataLayer.Context;
using DomainLayer.Models.Users;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CoreLayer.Services.Users;

public class UserService : BaseService, IUserService
{
    private readonly IUserRoleService _role;
    public UserService(AppDbContext context, IUserRoleService role) : base(context)
    {
        _role = role;
    }

    public async Task ChangePassword(ChangePasswordDto passwordDto)
    {
        var user = await GetSingleUser(passwordDto.UserId);

        if (user == null)
            throw new InvalidExpressionException();

        if (passwordDto.NewPassword != passwordDto.ConfirmPassword)
            throw new InvalidExpressionException();

        if (PasswordHelper.EncodePasswordMd5(passwordDto.CurrentPassword) != user.Password)
            throw new InvalidExpressionException("کلمه عبور نامعتبر است");

        user.Password = PasswordHelper.EncodePasswordMd5(user.Password);
        await EditUser(user);
    }

    public async Task<User> EditUser(EditProfileDto profileDto)
    {
        var user = await GetSingleUser(profileDto.UserId);
        if (user == null)
            throw new ArgumentNullException();
        if (!string.IsNullOrEmpty(profileDto.Email))
        {
            if (profileDto.Email != user.Email)
            {
                if (await IsEmailExist(profileDto.Email))
                    throw new Exception("ایمیل وارد شده تکراری است");
            }
        }
        if (profileDto.ImageFile != null)
        {
            if (profileDto.ImageFile.IsImage())
            {
                user.ImageName = await SaveFileInServer.SaveFile(profileDto.ImageFile, Directories.UserAvatar);
            }
        }

        user.IsCompleteProfile = true;
        user.BirthDate = profileDto.BirthDate;
        user.Name = profileDto.Name;
        user.Family = profileDto.Family;
        user.NationalCode = profileDto.NationalCode;
        user.Email = profileDto.Email;
        user.Gender = profileDto.Gender;

        Update(user);
        await Save();

        if (profileDto.ImageFile != null)
        {
            if (profileDto.ImageFile.IsImage())
                await SaveFileInServer.SaveFile(profileDto.ImageFile, Directories.UserAvatar);

        }

        return user;
    }

    public async Task<bool> EditUser(EditUserViewModel editModel)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == editModel.Id);
        if (user == null) return false;
        var oldImage = user.ImageName;

        user.Name = editModel.Name.SanitizeText();
        user.Family = editModel.Family.SanitizeText();
        user.IsActive = editModel.IsActive;
        user.Email = editModel.Email.SanitizeText();

        if (!string.IsNullOrWhiteSpace(editModel.NewPassword))
            user.Password = PasswordHelper.EncodePasswordMd5(editModel.NewPassword);

        try
        {
            if (editModel.UserImage != null)
            {
                if (editModel.UserImage.IsImage())
                {
                    user.ImageName = await SaveFileInServer.SaveFile(editModel.UserImage, Directories.UserAvatar);
                }
            }
            Update(user);
            await _role.EditUserRoles(editModel.UserRoles, user.Id);
            if (editModel.UserImage == null) return true;
            if (!editModel.UserImage.IsImage() || oldImage.ToLower() == "default.jpg") return true;
            DeleteFileFromServer.DeleteFile(oldImage, Directories.UserAvatar);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task EditUser(User user)
    {
        Update(user);
        await Save();
    }

    public async Task<User> GetSingleUser(long userId)
    {
        return await Table<User>()
                .Include(c => c.UserNotifications)
                .SingleOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User> GetSingleUser(long userId, bool withRelation)
    {
        return await Table<User>()
                .Include(c => c.UserNotifications)
                .Include(c => c.Addresses)
                .Include(c => c.Orders)
                .Include(c => c.UserCards)
                .Include(c => c.UserRoles)
                .ThenInclude(c => c.Role)
                .AsSplitQuery()
                .SingleOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User> GetSingleUser(string phoneNumber)
    {
        return await Table<User>()
                .Include(c => c.UserNotifications)
                .SingleOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }

    public async Task<User> GetSingleUser(string phoneNumber, bool withRelation)
    {
        return await Table<User>()
                .Include(c => c.UserNotifications)
                .Include(c => c.Orders)
                .ThenInclude(c => c.Details)
                .ThenInclude(c => c.Product)
                .SingleOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }

    public Task<User> GetSingleUserByEmail(string email)
    {
        return Table<User>().SingleOrDefaultAsync(u => u.Email == email);
    }

    public async Task<UsersViewModel> GetUsersForAdmin(int pageId, int take, string email, string name, long userId, string phone)
    {
        var result = Table<User>();
        if (!string.IsNullOrEmpty(email))
        {
            result = result.Where(r => r.Email.Contains(email));
        }
        if (!string.IsNullOrEmpty(name))
        {
            result = result.Where(r => r.Name.Contains(name) || r.Family.Contains(name));
        }
        if (!string.IsNullOrEmpty(phone))
        {
            result = result.Where(r => r.PhoneNumber.Contains(phone));
        }
        if (userId > 0)
        {
            result = result.Where(r => r.Id == userId);
        }
        var skip = (pageId - 1) * take;
        var model = new UsersViewModel()
        {
            Users = await result.OrderByDescending(d => d.CreationDate).Skip(skip).Take(take).ToListAsync(),
            UserId = userId,
            Name = name,
            Email = email
        };
        model.GeneratePaging(result, take, pageId);
        return model;
    }

    public async Task<bool> IsEmailExist(string email)
    {
        return await Table<User>().AnyAsync(u => u.Email == email);
    }

    public async Task<bool> IsUserExist(string phoneNumber)
    {
        return await Table<User>().AnyAsync(u => u.PhoneNumber == phoneNumber);
    }

    public async Task<bool> IsUserExist(long userId)
    {
        return await Table<User>().AnyAsync(u => u.Id == userId);
    }

    public async Task<User> LoginUser(LoginDto login)
    {
        var user = await Table<User>().SingleOrDefaultAsync(u => u.PhoneNumber == login.PhoneNumber);
        if (user == null)
            return null;

        if (user.Password != PasswordHelper.EncodePasswordMd5(login.Password))
            return null;

        if (!user.IsActive)
            throw new Exception("حساب کاربری شما غیرفعال است");

        return user;
    }

    public async Task RegisterUser(RegisterDto registerModel)
    {
        if (registerModel == null)
            throw new Exception("اطلاعات ناقص است");
        if (registerModel.Password != registerModel.RePassword)
            throw new Exception("کلمه های عبور یکسان نیستند");
        if (await IsUserExist(registerModel.PhoneNumber))
            throw new Exception("شماره تلفن وارد شده قبلا استفاده شده است");

        var user = new User()
        {
            Password = PasswordHelper.EncodePasswordMd5(registerModel.Password),
            BranchId = 0,
            IsActive = true,
            PhoneNumber = registerModel.PhoneNumber,
            IsCompleteProfile = false,
            LastSendActiveCodeDate = DateTime.Now,
            SecondActiveCode = TextHelper.GenerateCode(5),
            Presenter = await IsUserExist(registerModel.Presenter ?? "0") ? registerModel.Presenter : null
        };
        Insert(user);
        await Save();
    }

    public async Task ResetPassword(ResetPasswordDto resetPassword)
    {
        var user = await GetSingleUserByEmail(resetPassword.Email);
        if (user == null)
            throw new Exception();
        if (user.ActiveCode != resetPassword.ActiveCode)
            throw new Exception();

        var password = PasswordHelper.EncodePasswordMd5(resetPassword.Password);
        user.Password = password;
        user.ActiveCode = Guid.NewGuid().ToString();
        Update(user);
        await Save();
    }
}
