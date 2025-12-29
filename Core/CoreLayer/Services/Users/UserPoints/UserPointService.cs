using CoreLayer.DTOs;
using DataLayer.Context;
using DomainLayer.Models.Users;
using DomainLayer.Models.Wallets;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.Services.Users.UserPoints;

public class UserPointService : BaseService, IUserPointService
{
    public UserPointService(AppDbContext context) : base(context)
    {
    }

    public int BalancePoints(long userId)
    {
        var points = Table<UserPoint>().Where(x => x.UserId == userId);
        
        var typeOne = points.Where(x => x.Type == WalletType.واریز).Sum(x => x.Count);
        var typeTwo = points.Where(x => x.Type == WalletType.برداشت).Sum(x => x.Count);

        return typeOne - typeTwo;
    }

    public async Task ConvertPointToWalletAmount(long userId)
    {
        var points = BalancePoints(userId);
        var walletPoint = points * 3000;
        var wallet = new Wallet()
        {
            Amount = walletPoint,
            Authority = "Null",
            Description = $"تبدیل {points} کتو",
            IsFinally = true,
            PayWith = "Manually",
            UserId = userId,
            WalletType = WalletType.واریز,
            PaymentDate = DateTime.Now,
            CreationDate = DateTime.Now
        };
        var point = new UserPoint()
        {
            Count = points,
            CreationDate = DateTime.Now,
            UserId = userId,
            Type = WalletType.برداشت,
            Description = $"تبدیل {points} کتو به شارژ کیف پول"
        };
        Insert(wallet);
        Insert(point);
        await Save();
    }

    public async Task<UserPointsFilter> GetUserPoints(int pageId, int take, long userId)
    {
        var points = Table<UserPoint>()
                .Where(p => p.UserId == userId).OrderByDescending(d => d.CreationDate);

        var skip = (pageId - 1) * take;
        var model = new UserPointsFilter()
        {
            UserId = userId,
            Points = await points.Skip(skip).Take(take).ToListAsync()
        };
        model.GeneratePaging(points, take, pageId);
        return model;
    }
}
