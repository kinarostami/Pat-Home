using Common.Application.FileUtil;
using Common.Application.SecurityUtil;
using DataLayer.Context;
using DomainLayer.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.Services.Products.Groups;

public class ProductGroupService : BaseService, IProductGroupService
{
    public ProductGroupService(AppDbContext context) : base(context)
    {
    }

    public async Task AddGroup(ProductGroup group, IFormFile imageFile)
    {
        try
        {
            if (imageFile != null)
            {
                if (imageFile.IsImage())
                {
                    group.GroupImage = await SaveFileInServer.SaveFile(imageFile, Directories.ProductGroup);
                }
            }
            Insert(group);
            await Save();
        }
        catch
        {
            DeleteFileFromServer.DeleteFile(group.GroupImage, Directories.ProductGroup);
            throw new Exception();
        }
    }

    public async Task<bool> DeleteGroup(long groupId)
    {
        try
        {
            var group = await Table<ProductGroup>()
                .Include(g => g.SubGroups)
                .Include(g => g.ProductsMainGroup)
                .Include(g => g.ProductsParentGroup)
                .Include(g => g.ProductsSubParentGroup)
                .SingleOrDefaultAsync(g => g.Id == groupId);

            if (group == null) return false;
            if (group.ProductsMainGroup.Any()) return false;
            if (group.ProductsParentGroup.Any()) return false;
            if (group.ProductsSubParentGroup.Any()) return false;
            if (group.SubGroups.Any())
            {
                var groups = _context.ProductGroups;
                foreach (var item in group.SubGroups)
                {
                    Delete(item);
                    if (groups.Any(g => g.ParentId == item.Id))
                    {
                        foreach (var sub in groups.Where(g => g.ParentId == item.Id))
                        {
                            Delete(sub);
                        }
                    }
                }
            }
            Delete(group);
            await Save();
            DeleteFileFromServer.DeleteFile(group.GroupImage, Directories.ProductGroup);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task EditGroup(ProductGroup group, IFormFile imageFile)
    {
        var oldImage = group.GroupImage;
        if (imageFile != null)
        {
            if (imageFile.IsImage())
            {
                group.GroupImage = await SaveFileInServer.SaveFile(imageFile, Directories.ProductGroup);

            }
        }

        Update(group);
        await Save();
        if (imageFile != null)
        {
            if (imageFile.IsImage())
            {
                DeleteFileFromServer.DeleteFile(oldImage, Directories.ProductGroup);
            }
        }
    }

    public async Task<ProductGroup> GetGroupById(long groupId)
    {
        return await GetById<ProductGroup>(groupId);
    }

    public IQueryable<ProductGroup> GetGroupsByParentId(long parentId)
    {
        return Table<ProductGroup>().Where(g => g.ParentId == parentId);
    }

    public IQueryable<ProductGroup> GetProductGroups()
    {
        return Table<ProductGroup>();
    }
}
