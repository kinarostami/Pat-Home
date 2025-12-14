using InventoryManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Application.ApplicationSercices;

public interface IInventoryService
{
    Task AddInventory(AddInventoryCommand command);
    Task EditInventory(EditInventoryCommand command);
    Task DecreaseInventory(DecreaseInventoryCommand command);
    Task DeCreaseInventoryWithoutSave(DecreaseInventoryCommand command);
    Task IncreaseInventory(IncreaseInventoryCommand command);
    List<long> GetAvalibaleProducts();

    Task<List<InventoryDto>> GetProductInventories(long productId);
    Task<InventoryDto> GetById(long id);
    Task<InventoryDto> GetFirstInventoryByProductId(long productId);
    Task SaveChange();
    bool IsAvailable(long productId);
}
