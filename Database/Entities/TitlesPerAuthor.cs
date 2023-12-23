using System;
using System.Collections.Generic;

namespace Database.Entities;

public partial class TitlesPerAuthor
{
    public string Name { get; set; } = null!;

    public int? Age { get; set; }

    public int? Titles { get; set; }

    public int? InventoryValue { get; set; }
}
