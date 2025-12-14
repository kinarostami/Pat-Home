using InventoryManagement.Application.DTOs;
using InventoryManagement.Domain.Models;
using InventoryManagement.Domain.Models.InventoryAgg;
using InventoryManagement.Infrastructure.Persistent.EF.Context;

namespace InventoryManagement.Application.ApplicationSercices;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IInventoryDomainService _inventoryDomainService;
    private readonly InventoryContext _context;
    public InventoryService(IInventoryRepository inventoryRepository, IInventoryDomainService inventoryDomainService, InventoryContext context)
    {
        _inventoryRepository = inventoryRepository;
        _inventoryDomainService = inventoryDomainService;
        _context = context;
    }

    public async Task AddInventory(AddInventoryCommand command)
    {
        await _inventoryRepository.AddInventory(new Inventory(command.ProductId,command.Count,command.Price,command.ProductType,_inventoryDomainService));
        await _inventoryRepository.SaveChanges();
    }

    public async Task DecreaseInventory(DecreaseInventoryCommand command)
    {
        var inventory = await _inventoryRepository.GetById(command.Id);
        inventory.DecreaseInventory(command.Count);
    }

    public async Task DeCreaseInventoryWithoutSave(DecreaseInventoryCommand command)
    {
        var inventory = await _inventoryRepository.GetByIdTracking(command.Id);
        inventory.DecreaseInventory(command.Count);
    }

    public async Task EditInventory(EditInventoryCommand command)
    {
        var inventory = await _inventoryRepository.GetByIdTracking(command.Id);
        inventory.Edit(command.Price,command.Count,command.ProductType,_inventoryDomainService);
        await _inventoryRepository.SaveChanges();
    }

    public List<long> GetAvalibaleProducts()
    {
        return _context.Inventories.Where(i => i.Count > 1).Select(r => r.ProductId).ToList();
    }

    public async Task<InventoryDto> GetById(long id)
    {
        var res = await _inventoryRepository.GetById(id);
        if (res == null)
            return null;
        return new InventoryDto()
        {
            Count = res.Count,
            Price = res.UnitPrice,
            ProductId = res.ProductId,
            ProductType = res.Type,
            Id = res.Id
        };
    }

    public async Task<InventoryDto> GetFirstInventoryByProductId(long productId)
    {
        var res = await _inventoryRepository.GetFirstByProductId(productId);

        if (res == null)
            return null;
        return new InventoryDto()
        {
            Count = res.Count,
            Price = res.UnitPrice,
            ProductId = res.ProductId,
            ProductType = res.Type,
            Id = res.Id
        };
    }

    public async Task<List<InventoryDto>> GetProductInventories(long productId)
    {
        var inventories = await _inventoryRepository.GetProductInventories(productId);

        return inventories.Select(r => new InventoryDto()
        {
            Count = r.Count,
            ProductType = r.Type,
            Price = r.UnitPrice,
            ProductId = r.ProductId,
            Id = r.Id
        }).ToList();
    }

    public async Task IncreaseInventory(IncreaseInventoryCommand command)
    {
        var inventory = await _inventoryRepository.GetByIdTracking(command.Id);
        inventory.IncreaseInventory(command.Count);
        await _inventoryRepository.SaveChanges();
    }

    public bool IsAvailable(long productId)
    {
        return _context.Inventories.Any(p => p.Count > 0 && p.ProductId == productId);
    }

    public async Task SaveChange()
    {
        await _inventoryRepository.SaveChanges();
    }
}
