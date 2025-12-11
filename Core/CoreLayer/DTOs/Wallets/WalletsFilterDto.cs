using System.Collections.Generic;
using CoreLayer.DTOs.Pagination;
using DomainLayer.Models.Wallets;

namespace CoreLayer.DTOs.Wallets
{
    public class WalletsFilterDto : BasePaging
    {
       
        public List<Wallet> Wallet { get; set; }
        public long UserId { get; set; }

       
    }
}