using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? State { get; set; }

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string? ZipCode { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
