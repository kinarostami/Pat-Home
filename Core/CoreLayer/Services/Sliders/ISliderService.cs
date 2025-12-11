using System.Linq;
using System.Threading.Tasks;
using DomainLayer.Models.Sliders;
using Microsoft.AspNetCore.Http;

namespace CoreLayer.Services.Sliders
{
    public interface ISliderService
    {
        IQueryable<ShopSlider> GetSliders();
        Task AddSlider(ShopSlider slider, IFormFile sliderFile);
        Task EditSlider(ShopSlider slider, IFormFile sliderFile);
        Task<ShopSlider> GetSliderById(long sliderId);
        Task<ShopSlider> GetTrackingSlider(long sliderId);
        Task DeleteSlider(long sliderId);
    }
}