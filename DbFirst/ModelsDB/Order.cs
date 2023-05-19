using System;
using System.Collections.Generic;

namespace DbFirst.ModelsDB;

public partial class Order
{
    public int OId { get; set; }

    public DateOnly OrderDate { get; set; }

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();
}
