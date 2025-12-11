using DomainLayer.Models.Banners;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Banners;

public interface IBannerService
{
    IQueryable<Banner> GetBanners();
    Task<Banner> GetBannerById(long bannerId);
    Task<Banner> GetBannerById(long bannerId,bool isTracked);
    Task DeleteBanner(long bannerId);
    Task AddBanner(Banner banner,IFormFile bannerImage);
    Task EditBanner(Banner banner,IFormFile bannerImage);
}
