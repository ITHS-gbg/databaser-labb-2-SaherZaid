using System;
using System.Collections.Generic;

namespace Database.Entities;

public partial class Shift
{
    public int ShiftId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime ShiftStart { get; set; }

    public DateTime ShiftEnd { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
