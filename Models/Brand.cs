using System;
using System.Collections.Generic;

namespace HairCareStore.Models;

public partial class Brand
{
    public int BrandId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Logo { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
