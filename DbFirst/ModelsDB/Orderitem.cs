using System;
using System.Collections.Generic;

namespace DbFirst.ModelsDB;

public partial class Orderitem
{
    public int IId { get; set; }

    public int ProductId { get; set; }

    public int OrderId { get; set; }

    public decimal Amount { get; set; }

    public decimal Price { get; set; }

    public decimal? Total { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
