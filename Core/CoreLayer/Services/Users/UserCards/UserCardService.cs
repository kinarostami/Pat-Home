using Common.Application.SecurityUtil;
using CoreLayer.DTOs.Profile;
using DataLayer.Context;
using DomainLayer.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.Services.Users.UserCards;

public class UserCardService : BaseService, IUserCardService
{
    public UserCardService(AppDbContext context) : base(context)
    {
    }

    public async Task AddUserCard(UserCardViewModel cardModel)
    {
        var card = new UserCard()
        {
            AccountNumber = cardModel.AccountNumber.SanitizeText(),
            BankName = cardModel.BankName.SanitizeText(),
            CardNumber = cardModel.CardNumber.SanitizeText(),
            UserId = cardModel.UserId,
            ShabahNumber = cardModel.ShabahNumber.SanitizeText(),
            IsAccept = false,
            OwnerName = cardModel.OwnerName.SanitizeText(),
            CreationDate = DateTime.Now,
        };
        Insert(card);
        await Save();
    }

    public async Task<bool> DeleteUserCard(long cardId, long userId)
    {
        var card = await GetById<UserCard>(cardId);
        if (card == null || card.UserId != userId) return false;

        Delete(card);
        await Save();
        return true;
    }

    public async Task<bool> DeleteUserCard(long cardId)
    {
        var card = await GetById<UserCard>(cardId);
        if (card == null) return false;
        Delete(card);
        await Save();
        return true;
    }

    public async Task EditCard(UserCardViewModel cardModel)
    {
        var card = new UserCard()
        {
            AccountNumber = cardModel.AccountNumber.SanitizeText(),
            BankName = cardModel.BankName.SanitizeText(),
            CardNumber = cardModel.CardNumber.SanitizeText(),
            UserId = cardModel.UserId,
            ShabahNumber = cardModel.ShabahNumber.SanitizeText(),
            IsAccept = false,
            OwnerName = cardModel.OwnerName.SanitizeText(),
            Id = cardModel.CardId
        };
        Update(card);
        await Save();
    }

    public async Task<bool> EditCard(UserCard cardModel)
    {
        try
        {
            Update(cardModel);
            await Save();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public IQueryable<UserCard> GetCards()
    {
        return Table<UserCard>();
    }

    public async Task<UserCard> GetUserCard(long cardId)
    {
        return await GetById<UserCard>(cardId);
    }

    public async Task<UserCard> GetUserCard(long cardId, long userId)
    {
        return await _context.UserCards.SingleOrDefaultAsync(c => c.Id == cardId && c.UserId == userId);
    }

    public async Task<List<UserCard>> GetUserCards(long userId)
    {
        return await Table<UserCard>().Where(c => c.UserId == userId).ToListAsync();
    }
}
