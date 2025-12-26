using CoreLayer.Services.Products;
using DomainLayer.Models.Products;
using InventoryManagement.Application.ApplicationSercices;
using InventoryManagement.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Areas.Admin.Pages.Products.Inventory
{
    public class IndexModel : PageModel
    {
        private readonly IInventoryService _inventoryService;
        private readonly IProductService _productService;

        public IndexModel(IInventoryService inventoryService, IProductService productService)
        {
            _inventoryService = inventoryService;
            _productService = productService;
        }
        public Product Product { get; set; }
        public List<InventoryDto> Inventories { get; set; }
        public async Task<IActionResult> OnGet(long productId)
        {
            Product = await _productService.GetProductById(productId);
            if (Product == null)
                return RedirectToPage("Index");

            Inventories = await _inventoryService.GetProductInventories(productId);
            return Page();
        }
    }
}
