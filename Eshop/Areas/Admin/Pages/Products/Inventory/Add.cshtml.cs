using System.ComponentModel.DataAnnotations;
using CoreLayer.Services.Products;
using CoreLayer.Utilities;
using DomainLayer.Models.Products;
using DomainLayer.Models.Roles;
using Eshop.Infrastructure;
using InventoryManagement.Application.ApplicationSercices;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Areas.Admin.Pages.Products.Inventory
{
    [BindProperties]
    //[ValidateAntiForgeryToken]
    //[PermissionChecker(Permissions.مدیریت_محصولات)]
    public class AddModel : PageUtil
    {
        private readonly IInventoryService _inventoryService;
        private readonly IProductService _productService;
        public AddModel(IInventoryService inventoryService, IProductService productService)
        {
            _inventoryService = inventoryService;
            _productService = productService;
        }

        [Display(Name = "تعداد موجود")]
        public int Count { get; set; }

        [Display(Name = "قیمت واحد")]
        public int Price { get; set; }

        [Display(Name = "نوع محصول")]
        public ProductType Type { get; set; }

        public Product Product { get; set; }
        public async Task<IActionResult> OnGet(long productId)
        {
            Product = await _productService.GetProductById(productId);
            if (Product == null)
                return RedirectToPage("Index");

            return Page();
        }

        public async Task<IActionResult> OnPost(long productId)
        {
            return await TryCatch(async () =>
            {
                await _inventoryService.AddInventory(new AddInventoryCommand()
                {
                    Count = Count,
                    Price = Price,
                    ProductId = productId,
                    ProductType = Type
                });
            }, "/admin/Products/Inventory/" + productId);
        }
    }
}
