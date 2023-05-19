using System;
using System.Collections.Generic;

namespace DbFirst.ModelsDB;

public partial class Product
{
    public int PId { get; set; }

    public string PName { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();
}
