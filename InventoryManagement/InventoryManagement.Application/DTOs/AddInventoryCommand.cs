using InventoryManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Application.DTOs;

public class AddInventoryCommand
{
    public long ProductId { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }
    public ProductType ProductType { get; set; }
}
