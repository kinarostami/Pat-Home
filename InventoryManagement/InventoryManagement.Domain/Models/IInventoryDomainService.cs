using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Domain.Models;

public interface IInventoryDomainService
{
    bool IsInventoryExist(ProductType type, long productId);
}
