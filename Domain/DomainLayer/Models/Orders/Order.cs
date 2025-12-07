using DomainLayer.Models.Orders.DomainServices;
using DomainLayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Orders;

public class Order : BaseEntity
{
    public long UserId { get; set; }
    public int Price { get; set; }
    public int? Discount { get; set; }
    public int? DiscountPercentage { get; set; }
    public string DiscountTitle { get; set; }
    public string SendTypeTitle { get; set; }
    public string TrackingCode { get; set; }
    public long? RefId { get; set; }
    public string Authority { get; set; }
    public int ShippingCost { get; private set; }
    public int WalletAmount { get; private set; }
    public int ItemCount { get; set; }
    public bool IsFinally { get; set; }
    public bool IsSendFactor { get; set; }
    public bool ByWallet { get; set; }
    public bool IsSend { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime SendDate { get; set; }

    public void SetWalletAmoutn(int amount)
    {
        WalletAmount = amount;
        if (WalletAmount > TotalPrice)
            WalletAmount = TotalPrice;

        if (amount > 0)
            ByWallet = true;
        else
            ByWallet = false;
    }
    public int GetTotalPriceForPay()
    {
        return TotalPrice - WalletAmount;
    }
    public int TotalPrice
    {
        get
        {
            var price = Price + ShippingCost - GetDiscount;

            return price < 0 ? 0 : price;
        }
    }

    public int GetDiscount
    {
        get
        {
            if (DiscountPercentage != null)
                return Price * (DiscountPercentage ?? 0) / 100;

            return Discount ?? 0;
        }
    }

    public void CalculateShippingConst(IShippingCostDomainService _domainService)
    {
        var totalWeight = Convert.ToInt32(Details.Sum(d => d.KiloGram));
        var shippingCosts = _domainService.GetShippingCosts();
        var shippingCost = CalculateShippingConst(totalWeight, shippingCosts);
        ShippingCost = shippingCost;
    }

    private int CalculateShippingConst(int totalWeight, List<ShippingCost> costs)
    {
        var shippingCost = 45000;

        const int NimKilo = 500;
        const int YekKilo = 1000;
        const int YekKiloVa500Gram = 1500;
        const int DoKiloVa500Gram = 2500;
        const int SeKilo = 3000;

        if (totalWeight is > 0 and <= NimKilo)
            shippingCost = costs.FirstOrDefault(f => f.Type == ShippingCostType.NimKilo)?.Cost ?? 40000;

        else if (totalWeight is > NimKilo and <= YekKilo)
            shippingCost = costs.FirstOrDefault(f => f.Type == ShippingCostType.YekKilo)?.Cost ?? 50000;

        else if (totalWeight is > YekKilo and <= YekKiloVa500Gram)
            shippingCost = costs.FirstOrDefault(f => f.Type == ShippingCostType.YekKiloVa500Gram)?.Cost ?? 60000;

        else if (totalWeight is > YekKiloVa500Gram and < DoKiloVa500Gram)
            shippingCost = costs.FirstOrDefault(f => f.Type == ShippingCostType.YekKiloVa500Gram_Ta_DokiloVa500Gram)?.Cost ?? 70000;

        else if (totalWeight == DoKiloVa500Gram)
            shippingCost = costs.FirstOrDefault(f => f.Type == ShippingCostType.DoKiloVa500Gram)?.Cost ?? 75000;

        else if (totalWeight is > DoKiloVa500Gram and < SeKilo)
            shippingCost = 80000;

        else if (totalWeight >= SeKilo)
            shippingCost = costs.FirstOrDefault(f => f.Type == ShippingCostType.SeKilo)?.Cost ?? 50000;

        return shippingCost;
    }

    #region Relations
    public User User { get; set; }
    public OrderAddress Address { get; set; }
    public ICollection<OrderDetail> Details { get; set; }
    #endregion
}
public enum OrderStatus
{
    در_انتظار_پرداخت = 1,
    پرداخت_شده = 2,
    ارسال_شده = 3,
    تحویل_به_مشتری = 4
}
