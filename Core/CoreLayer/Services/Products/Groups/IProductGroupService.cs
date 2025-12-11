using DomainLayer.Models.Products;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Products.Groups;

public interface IProductGroupService
{
    IQueryable<ProductGroup> GetProductGroups();
    Task<bool> DeleteGroup(long groupId);
    Task<ProductGroup> GetGroupById(long groupId);
    IQueryable<ProductGroup> GetGroupsByParentId(long parentId);
    Task AddGroup(ProductGroup group, IFormFile imageFile);
    Task EditGroup(ProductGroup group, IFormFile imageFile);
}
