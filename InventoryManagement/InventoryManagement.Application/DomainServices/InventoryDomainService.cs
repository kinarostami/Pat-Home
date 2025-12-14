using InventoryManagement.Domain;
using InventoryManagement.Domain.Models;
using InventoryManagement.Infrastructure.Persistent.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Application.DomainServices;

public class InventoryDomainService : IInventoryDomainService
{
    private readonly InventoryContext _context;

    public InventoryDomainService(InventoryContext context)
    {
        _context = context;
    }
    public bool IsInventoryExist(ProductType type, long productId)
    {
        return _context.Inventories.Any(x => x.Type == type && x.ProductId == productId);
    }
}
