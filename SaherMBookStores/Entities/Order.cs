using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly? OrderDate { get; set; }

    public TimeOnly? OrderTime { get; set; }

    public string OrderStatus { get; set; } = null!;

    public string OrderType { get; set; } = null!;

    public string? ReferenceNumber { get; set; }

    public int? CustomerNo { get; set; }

    public int? StoreNo { get; set; }

    public int? OrderPrice { get; set; }

    public virtual Customer? CustomerNoNavigation { get; set; }

    public virtual Book? ReferenceNumberNavigation { get; set; }

    public virtual Store? StoreNoNavigation { get; set; }
}
