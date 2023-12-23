using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class InventoryBalance
{
    public int StoreId { get; set; }

    public string Isbn13 { get; set; } = null!;

    public int NoOfProducts { get; set; }

    public virtual Book Book { get; set; } //= null!;

    public virtual Store Store { get; set; } //= null!;
}
