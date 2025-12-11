using System;
using System.Threading.Tasks;
using DataLayer.Context;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.Services.AboutUses
{
    public class AboutUsService : BaseService, IAboutUsService
    {
        public AboutUsService(AppDbContext context) : base(context)
        {
        }
        public async Task AddOrEdit(AboutUs entity)
        {
            entity.LastModify = DateTime.Now;
            if (entity.Id >= 1)
            {
                _context.Update(entity);
                await Save();
                entity.LastModify = DateTime.Now;
                return;
            }
            await _context.AboutUs.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<AboutUs> GetAboutUs()
        {
            return await _context.AboutUs.FirstOrDefaultAsync();
        }


    }
}