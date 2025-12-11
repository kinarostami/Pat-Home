using System.Threading.Tasks;
using CoreLayer.DTOs.Pagination;
using CoreLayer.DTOs.Wallets;
using DomainLayer.Models.Wallets;
using Microsoft.EntityFrameworkCore.Query;

namespace CoreLayer.Services.Wallets
{
    public interface IWalletService
    {
        Task<WalletsFilterDto> GetWallets(int pageId, long userId, int take);
        Task<int> BalanceWallet(long userId);
        Task<Wallet> GetWalletById(long id);
        Task<long> AddWallet(Wallet wallet);
        Task FinallyWallet(Wallet wallet);
        Task DeleteWallet(Wallet wallet);
    }
}