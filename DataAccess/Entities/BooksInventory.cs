using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class BooksInventory
{
    public int StoreId { get; set; }

    public string Booktitle { get; set; } = null!;

    public string Isbn13 { get; set; } = null!;

    public int NoOfProducts { get; set; }
}
