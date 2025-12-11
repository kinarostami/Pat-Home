using System.Threading.Tasks;
using CoreLayer.DTOs;

namespace CoreLayer.Services.Users.UserPoints;

public interface IUserPointService
{
    int BalancePoints(long userId);
    Task<UserPointsFilter> GetUserPoints(int pageId, int take, long userId);
    Task ConvertPointToWalletAmount(long userId);
}
