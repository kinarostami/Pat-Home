using Common.Application.FileUtil;
using Common.Application.SecurityUtil;
using DataLayer.Context;
using DomainLayer.Models.Banners;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.Services.Banners;

public class BannerService : BaseService, IBannerService
{
    public BannerService(AppDbContext context) : base(context)
    {
    }

    public async Task AddBanner(Banner banner, IFormFile bannerImage)
    {
        if(bannerImage != null)
        {
            if (bannerImage.IsImage())
            {
                banner.ImageName = await SaveFileInServer.SaveFile(bannerImage,Directories.Banners);
            }
        }
        Insert(banner);
        await Save();
    }

    public async Task DeleteBanner(long bannerId)
    {
        var banner = await GetBannerById(bannerId,true);
        Delete(banner);
        await Save();
        DeleteFileFromServer.DeleteFile(banner.ImageName, Directories.Banners);
    }

    public async Task EditBanner(Banner banner, IFormFile bannerImage)
    {
        var oldImage = banner.ImageName;
        if (bannerImage != null)
        {
            if (bannerImage.IsImage())
            {
                banner.ImageName = await SaveFileInServer.SaveFile(bannerImage, Directories.Banners);
            }
        }
        Update(banner);
        await Save();
        if (bannerImage != null)
        {
            if (bannerImage.IsImage())
            {
                DeleteFileFromServer.DeleteFile(oldImage, Directories.Banners);
            }
        }
    }

    public async Task<Banner> GetBannerById(long bannerId)
    {
        return await GetById<Banner>(bannerId);
    }

    public async Task<Banner> GetBannerById(long bannerId, bool isTracked)
    {
        return await _context.Banners.SingleOrDefaultAsync(x => x.Id == bannerId);
    }

    public IQueryable<Banner> GetBanners()
    {
        return Table<Banner>();
    }
}
