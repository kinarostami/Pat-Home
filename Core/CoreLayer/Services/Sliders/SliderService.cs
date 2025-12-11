using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Application.FileUtil;
using Common.Application.SecurityUtil;
using DataLayer.Context;
using DomainLayer.Models.Sliders;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.Services.Sliders
{
    public class SliderService : BaseService, ISliderService
    {
        public SliderService(AppDbContext context) : base(context)
        {
        }

        public IQueryable<ShopSlider> GetSliders()
        {
            return Table<ShopSlider>();
        }

        public async Task AddSlider(ShopSlider slider, IFormFile sliderFile)
        {
            if (sliderFile == null)
                throw new Exception("عکس اسلایدر را وارد کنید");

            if (!sliderFile.IsImage())
                throw new Exception("عکس اسلایدر را وارد کنید");


            slider.ImageName = await SaveFileInServer.SaveFile(sliderFile, Directories.Sliders);
            slider.CreationDate = DateTime.Now;
            Insert(slider);
            await Save();
        }

        public async Task EditSlider(ShopSlider slider, IFormFile sliderFile)
        {
            var oldImage = slider.ImageName;
            if (sliderFile != null)
            {
                if (sliderFile.IsImage())
                {
                    slider.ImageName = await SaveFileInServer.SaveFile(sliderFile, Directories.Sliders);
                }
            }
            Update(slider);
            await Save();
            if (sliderFile != null)
            {
                if (sliderFile.IsImage())
                {
                    DeleteFileFromServer.DeleteFile(oldImage, Directories.Sliders);
                }
            }
        }

        public async Task<ShopSlider> GetSliderById(long sliderId)
        {
            return await GetById<ShopSlider>(sliderId);
        }

        public async Task<ShopSlider> GetTrackingSlider(long sliderId)
        {
            return await _context.ShopSliders.SingleOrDefaultAsync(s => s.Id == sliderId);
        }

        public async Task DeleteSlider(long sliderId)
        {
            var slider = await GetSliderById(sliderId);
            Delete(slider);
            await Save();
            DeleteFileFromServer.DeleteFile(slider.ImageName, Directories.Sliders);
        }
    }
}