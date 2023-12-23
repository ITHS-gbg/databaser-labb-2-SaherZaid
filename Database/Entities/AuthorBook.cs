using System;
using System.Collections.Generic;

namespace Database.Entities;

public partial class AuthorBook
{
    public int? AuthorId { get; set; }

    public string? BookId { get; set; }

    public virtual Author? Author { get; set; }

    public virtual Book? Book { get; set; }
}
