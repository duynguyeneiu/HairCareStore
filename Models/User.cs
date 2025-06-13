using System;
using System.Collections.Generic;

namespace HairCareStore.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Description { get; set; }

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public int? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UserCode { get; set; }

    public bool? IsLocked { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsActive { get; set; }

    public string? ActiveCode { get; set; }

    public string? Avatar { get; set; }

    public int RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual ICollection<Order>? Orders { get; set; }
}
