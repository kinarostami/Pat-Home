using CoreLayer.DTOs.Profile;
using DomainLayer.Models.Users;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayer.Services.Users.UserCards;

public interface IUserCardService
{
    IQueryable<UserCard> GetCards();
    Task<List<UserCard>> GetUserCards(long userId);
    Task<UserCard> GetUserCard(long cardId);
    Task<UserCard> GetUserCard(long cardId, long userId);
    Task EditCard(UserCardViewModel cardModel);
    Task<bool> EditCard(UserCard cardModel);
    Task AddUserCard(UserCardViewModel cardModel);
    Task<bool> DeleteUserCard(long cardId, long userId);
    Task<bool> DeleteUserCard(long cardId);
}
