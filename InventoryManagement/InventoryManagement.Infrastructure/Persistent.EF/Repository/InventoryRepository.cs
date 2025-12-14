using InventoryManagement.Domain.Models.InventoryAgg;
using InventoryManagement.Infrastructure.Persistent.EF.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Infrastructure.Persistent.EF.Repository;

public class InventoryRepository : IInventoryRepository
{
    private readonly InventoryContext _context;

    public InventoryRepository(InventoryContext context)
    {
        _context = context;
    }

    public async Task AddInventory(Inventory entity)
    {
        await _context.AddAsync(entity);
    }

    public async Task<bool> Any(long id)
    {
        return await _context.Inventories.AnyAsync(x => x.Id == id);
    }

    public async Task<Inventory> GetById(long id)
    {
        return await _context.Inventories.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Inventory> GetByIdTracking(long id)
    {
        return await _context.Inventories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Inventory> GetFirstByProductId(long productId)
    {
        return await _context.Inventories.FirstOrDefaultAsync(x => x.ProductId == productId);
    }

    public async Task<List<Inventory>> GetProductInventories(long productId)
    {
        return await _context.Inventories.Where(x => x.ProductId == productId).ToListAsync();
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public void UpdateInventory(Inventory entity)
    {
        _context.Update(entity);
    }
}
