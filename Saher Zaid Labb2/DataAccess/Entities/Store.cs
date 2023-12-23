using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Store
{
    public int StoreID { get; set; }
    public string StoreName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<InventoryBalance> InventoryBalances { get; set; } = new List<InventoryBalance>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public List<Book> Books { get; set; } = new();
}
