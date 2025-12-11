using System.Threading.Tasks;
using DomainLayer.Models;

namespace CoreLayer.Services.AboutUses
{
    public interface IAboutUsService
    {
        Task AddOrEdit(AboutUs entity);
        Task<AboutUs> GetAboutUs();
    }
}