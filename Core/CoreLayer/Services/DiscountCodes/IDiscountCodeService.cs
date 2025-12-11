using DomainLayer.Models.DiscountCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.DiscountCodes;

public interface IDiscountCodeService
{
    IQueryable<DiscountCode> GetAllCodes();
    Task<DiscountCode> GetDiscountCode(long codeId);
    Task<DiscountCode> GetDiscountCode(long codeId,bool isTracked);
    Task<DiscountCode> GetDiscountCode(string codeTitle);
    Task AddNewCode(DiscountCode code);
    Task EditNewCode(DiscountCode code);
    Task DeleteCode(long codeId);
    Task<bool> IsCodeExist(long codeId);
    Task<bool> IsCodeExist(string codeTitle);
}
