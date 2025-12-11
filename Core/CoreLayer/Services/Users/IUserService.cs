using CoreLayer.DTOs.Admin.Users;
using CoreLayer.DTOs.Auth;
using CoreLayer.DTOs.Profile;
using DomainLayer.Models.Roles;
using DomainLayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Users;

public interface IUserService
{
    Task<UsersViewModel> GetUsersForAdmin(int pageId, int take, string email, string name, long userId, string phone);
    Task RegisterUser(RegisterDto registerModel);
    Task<User> GetSingleUser(long userId);
    Task<User> GetSingleUser(long userId, bool withRelation);
    Task<User> GetSingleUser(string phoneNumber);
    Task<User> GetSingleUserByEmail(string email);
    Task<User> GetSingleUser(string phoneNumber, bool withRelation);
    Task<User> LoginUser(LoginDto login);
    Task<User> EditUser(EditProfileDto profileDto);
    Task<bool> EditUser(EditUserViewModel editModel);
    Task EditUser(User user);
    Task ChangePassword(ChangePasswordDto passwordDto);
    Task ResetPassword(ResetPasswordDto resetPassword);
    Task<bool> IsUserExist(string phoneNumber);
    Task<bool> IsEmailExist(string email);
    Task<bool> IsUserExist(long userId);
}
