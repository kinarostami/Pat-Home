using DomainLayer.Models;
using DomainLayer.Models.Roles;
using DomainLayer.Models.Users;

namespace CoreLayer.Services
{
    public interface IAppContext
    {
        Task AddOrEditPopUp(PopUpModel popUp);
        Task<PopUpModel> GetPopUp();
        string SiteBaseUrl { get; }
        bool CheckPermission(Permissions permission);
        SiteSetting SiteSettings { get; }
        Task<User> CurrentUser();
        public bool IsError { get; }
        public bool IsSuccess { get; }
        public long CurrentUserId { get; }
        void SetSiteSettings();
    }
}