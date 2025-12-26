using System.ComponentModel.DataAnnotations;
using Eshop.Infrastructure;
using InventoryManagement.Application.ApplicationSercices;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Areas.Admin.Pages.Products.Inventory
{
    //[ValidateAntiForgeryToken]
    [BindProperties]
    public class EditModel : PageUtil
    {
        private readonly IInventoryService _inventoryService;
        public EditModel(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [Display(Name = "تعداد موجود")]
        public int Count { get; set; }

        [Display(Name = "قیمت واحد")]
        public int Price { get; set; }

        [Display(Name = "نوع محصول")]
        public ProductType Type { get; set; }

        public long ProductId { get; set; }
        public async Task<IActionResult> OnGet(long id)
        {
            var inventory = await _inventoryService.GetById(id);
            if (inventory == null)
                return RedirectToPage("Index");

            Type = inventory.ProductType;
            Price = inventory.Price;
            Count = inventory.Count;
            ProductId = inventory.ProductId;
            return Page();
        }

        public async Task<IActionResult> OnPost(long id)
        {
            return await TryCatch(async () =>
            {
                await _inventoryService.EditInventory(new EditInventoryCommand()
                {
                    Count = Count,
                    Price = Price,
                    ProductType = Type,
                    Id = id
                });
            }, "/admin/Products/Inventory/" + ProductId);
        }
    }
}
