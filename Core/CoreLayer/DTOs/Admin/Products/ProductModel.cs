using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DomainLayer.Models.Products;
using Microsoft.AspNetCore.Http;

namespace CoreLayer.DTOs.Admin.Products
{

    public class AddProductViewModel
    {
        public ProductModel Product { get; set; }
        public List<string> Keys { get; set; } = new List<string>();
        public List<string> Values { get; set; } = new List<string>();
        public IFormFile CoverImage { get; set; }
        public IFormFile CoverImageForBitMap { get; set; }
        public List<IFormFile> Images { get; set; }
    }

    public class ProductModel : IValidatableObject
    {
        public int Gram { get; set; }
        public int DiscountPercentage { get; set; } = 0;
        public long ProductId { get; set; } = 0;

        [Required(ErrorMessage = "عنوان محصول را وارد کنید")]
        public string ProductTitle { get; set; }

        [Required(ErrorMessage = "توضیحات محصول را وارد کنید")]
        public string ProductDescription { get; set; }

        public long UserId { get; set; } = 0;

        [Required(ErrorMessage = "دسته بندی را انتخاب کنید")]
        public long GroupId { get; set; }

        [Required(ErrorMessage = "دسته بندی را انتخاب کنید")]
        public long ParentGroupId { get; set; }
        public long? SubParentGroupId { get; set; }
        public ProductStatus Status { get; set; } = ProductStatus.Active;

        [Required(ErrorMessage = "Meta Description اجباری است")]
        [MaxLength(350, ErrorMessage = "Meta Description نمی تواند بیشتر از 350 کاراکتر باشد")]
        public string MetaDescription { get; set; }
        [Required(ErrorMessage = " کلمات کلدی را وارد کنید")]
        public string Tags { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (GroupId == 0)
            {
                yield return new ValidationResult("دسته بندی را انتخاب کنید", new[] { nameof(GroupId) });
            }
            if (ParentGroupId == 0)
            {
                yield return new ValidationResult("دسته بندی را انتخاب کنید", new[] { nameof(ParentGroupId) });
            }
        }

        public string ImageName { get; set; }
        public List<ProductGallery> ProductGalleries { get; set; }
    }
}
