using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class BookstoreInfo
{
    public int CustomerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int? TotalOrdes { get; set; }

    public int? TotalOrdersPrice { get; set; }
}
