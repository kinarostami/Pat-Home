using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLayer.Services.Products.Groups;
using DomainLayer.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly IProductGroupService _group;

        public IndexModel(IProductGroupService @group)
        {
            _group = @group;
        }
        public IQueryable<ProductGroup> ProductGroups { get; set; }
        public void OnGet()
        {
            ProductGroups = _group.GetProductGroups().Include(c=>c.SubGroups);
        }
    }
}
