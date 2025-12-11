using DataLayer.Context;
using DomainLayer.Models.DiscountCodes;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.Services.DiscountCodes;

public class DiscountCodeService : BaseService, IDiscountCodeService
{
    public DiscountCodeService(AppDbContext context) : base(context)
    {
    }

    public async Task AddNewCode(DiscountCode code)
    {
        Insert(code);
        await Save();
    }

    public async Task DeleteCode(long codeId)
    {
        var code = await GetDiscountCode(codeId);
        Delete(code);
        await Save();
    }

    public async Task EditNewCode(DiscountCode code)
    {
        Update(code);
        await Save();
    }

    public IQueryable<DiscountCode> GetAllCodes()
    {
        return Table<DiscountCode>();
    }

    public async Task<DiscountCode> GetDiscountCode(long codeId)
    {
        return await GetById<DiscountCode>(codeId);
    }

    public async Task<DiscountCode> GetDiscountCode(long codeId, bool isTracked)
    {
        if (isTracked)
        {
            return await _context.DiscountCodes.SingleOrDefaultAsync(x => x.Id == codeId);
        }
        return await GetDiscountCode(codeId);
    }

    public async Task<DiscountCode> GetDiscountCode(string codeTitle)
    {
        return await Table<DiscountCode>().SingleOrDefaultAsync(c => c.CodeTitle == codeTitle);
    }

    public async Task<bool> IsCodeExist(long codeId)
    {
        return await _context.DiscountCodes.AnyAsync(c => c.Id == codeId);
    }

    public async Task<bool> IsCodeExist(string codeTitle)
    {
        return await _context.DiscountCodes.AnyAsync(c => c.CodeTitle == codeTitle);
    }
}
