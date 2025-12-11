using System.Collections.Generic;
using CoreLayer.DTOs.Pagination;
using DomainLayer.Models;

namespace CoreLayer.DTOs
{
    public class LogsViewModel:BasePaging
    {
        public List<Log> Logs { get; set; }
    }
}