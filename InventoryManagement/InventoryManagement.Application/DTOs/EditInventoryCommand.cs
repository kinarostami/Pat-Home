using InventoryManagement.Domain;

namespace InventoryManagement.Application.DTOs;

public class EditInventoryCommand
{
    public long Id { get; set; }
    public int Price { get; set; }
    public int Count { get; set; }
    public ProductType ProductType { get; set; }
}