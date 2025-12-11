using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Domain.Models.InventoryAgg;

public class Inventory : BaseEntity
{
    public Inventory()
    {
        
    }

    public Inventory(long productId, int count, int unitPrice, ProductType type,IInventoryDomainService domainService)
    {
        if (domainService.IsInventoryExist(type, productId))
            throw new InvalidDataException();

        ProductId = productId;
        Count = count;
        UnitPrice = unitPrice;
        Type = type;
    }


    public long ProductId { get; set; }
    public int Count { get; set; }
    public int UnitPrice { get; set; }
    public ProductType Type { get; set; }

    public void Edit(int price, int count, ProductType type, IInventoryDomainService domainService)
    {
        if (Type != type)
            if (domainService.IsInventoryExist(type, ProductId))
                throw new InvalidDataException();

        UnitPrice = price;
        Count = count;
        Type = type;
    }

    public void IncreaseInventory(int count)
    {
        Count += count;
    }
    public void DecreaseInventory(int count)
    {
        Count -= count;
    }
}
