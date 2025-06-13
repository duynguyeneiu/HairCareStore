using System;
using System.Collections.Generic;

namespace HairCareStore.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public string? Avatar { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
