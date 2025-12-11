using InventoryManagement.Domain;

namespace InventoryManagement.Application.DTOs;

public class InventoryDto
{
    public long ProductId { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }
    public ProductType ProductType { get; set; }
    public long Id { get; set; }
}
