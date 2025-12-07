using System;
using System.Collections.Generic;
using System.Diagnostics;
using DomainLayer.Models.Products;

namespace DomainLayer.Models.Orders
{
    public class OrderDetail : BaseEntity
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public long InventoryId { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public int TotalPrice => Price * Count;
        public StackType StackType { get; set; }

        public int KiloGram
        {
            get
            {
                if (Product == null) return 0;

                switch (StackType)
                {
                    case StackType.NimKilo:
                        return 500 * Count;
                    case StackType.YekKilo:
                        return 1000 * Count;
                    case StackType.None:
                        return Product.Gram * Count;
                    default:
                        return 0;
                }
            }
        }

        #region Relations

        public Order Order { get; set; }
        public Product Product { get; set; }
        #endregion
        public string GetWeight()
        {
            switch (StackType)
            {
                case StackType.NimKilo:
                    return "500 گرم";
                case StackType.YekKilo:
                    return "1000 گرم (1 کیلو)";
                case StackType.None:
                    return $"{Product.Gram} گرم";
            }

            return "";
        }
    }


    public enum StackType
    {
        None = 0,
        YekKilo = 1,
        NimKilo = 2,
    }
}