using System;
using System.Collections.Generic;
namespace DataAccess.Entities;

public partial class Book
{
    public string Title { get; set; } = null!;

    public string Language { get; set; } = null!;

    public int Price { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public string Isbn13 { get; set; } = null!;

    public int AuthorNo { get; set; }

    public virtual Author? AuthorNoNavigation { get; set; }

    public virtual ICollection<InventoryBalance> InventoryBalances { get; set; } = new List<InventoryBalance>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public List<Store> Stores { get; set; } = new();

}
