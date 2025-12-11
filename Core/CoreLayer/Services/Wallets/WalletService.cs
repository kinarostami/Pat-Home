using System.Linq;
using System.Threading.Tasks;
using CoreLayer.DTOs.Pagination;
using CoreLayer.DTOs.Wallets;
using DataLayer.Context;
using DomainLayer.Models.Wallets;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.Services.Wallets
{
    public class WalletService : BaseService, IWalletService
    {
        public WalletService(AppDbContext context) : base(context)
        {
        }

        public async Task<WalletsFilterDto> GetWallets(int pageId, long userId, int take)
        {
            var res = Table<Wallet>().Where(w => w.IsFinally);

            if (userId > 0)
            {
                res = res.Where(r => r.UserId == userId);
            }

            var skip = (pageId - 1) * take;
            var model = new WalletsFilterDto()
            {
                Wallet = await res.OrderByDescending(d=>d.PaymentDate).Skip(skip).Take(take).ToListAsync()
            };
            model.GeneratePaging(res, take, pageId);

            return model;
        }

        public async Task<int> BalanceWallet(long userId)
        {
            var userWallets = Table<Wallet>().Where(w => w.UserId == userId && w.IsFinally);

            var withdraw = await userWallets.Where(w => w.WalletType == WalletType.برداشت).SumAsync(w => w.Amount);
            var deposit = await userWallets.Where(w => w.WalletType == WalletType.واریز).SumAsync(w => w.Amount);
            return deposit - withdraw;
        }

        public async Task<Wallet> GetWalletById(long id)
        {
            return await GetById<Wallet>(id);
        }

        public async Task<long> AddWallet(Wallet wallet)
        {
            Insert(wallet);
            await Save();
            return wallet.Id;
        }

        public async Task FinallyWallet(Wallet wallet)
        {
            wallet.IsFinally = true;
            Update(wallet);
            await Save();
        }

        public async Task DeleteWallet(Wallet wallet)
        {
            Delete(wallet);
            await Save();
        }
    }
}