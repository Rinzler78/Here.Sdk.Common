namespace Here.Sdk.Common.Routing;

/// <summary>Payment method accepted at a toll point.</summary>
public enum PaymentMethod
{
    /// <summary>Exact change coin payment.</summary>
    AutomaticCoin = 0,
    /// <summary>Cash payment.</summary>
    Cash = 1,
    /// <summary>Credit or debit card.</summary>
    CreditCard = 2,
    /// <summary>Discount card or membership.</summary>
    DiscountCard = 3,
    /// <summary>No payment required.</summary>
    NoPayment = 4,
    /// <summary>Pass or subscription.</summary>
    PassSubscription = 5,
    /// <summary>Electronic transponder (e.g. E-ZPass).</summary>
    Transponder = 6,
    /// <summary>Stored-value travel card.</summary>
    TravelCard = 7,
    /// <summary>Video-based billing (license plate recognition).</summary>
    VideoToll = 8,
    /// <summary>Voucher or coupon.</summary>
    Voucher = 9,
}
