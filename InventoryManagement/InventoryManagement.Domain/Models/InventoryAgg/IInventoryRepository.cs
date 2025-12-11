using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Domain.Models.InventoryAgg;

public interface IInventoryRepository
{
    Task<List<Inventory>> GetProductInventories(long productId);
    Task AddInventory(Inventory entity);
    void UpdateInventory(Inventory entity);
    Task<bool> Any(long id);
    Task<Inventory> GetById(long id);
    Task<Inventory> GetFirstByProductId(long productId);
    Task<Inventory> GetByIdTracking(long id);
    Task SaveChanges();
}
