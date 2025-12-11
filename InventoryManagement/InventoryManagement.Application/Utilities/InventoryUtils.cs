using InventoryManagement.Domain;

namespace InventoryManagement.Application.Utilities
{
    public static class InventoryUtils
    {
        public static string GetNameOfProductType(this ProductType type)
        {
            switch (type)
            {
                case ProductType.Unit:
                    return "واحد";
                case ProductType.NimKilo:
                    return "نیم کیلو";
                case ProductType.YekKio:
                    return "یک کیلو";
            }
            return "";
        }
    }
}