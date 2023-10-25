using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using XoomCore.Core.Enum;
using XoomCore.Core.Shared;

namespace XoomCore.Core.Entities.AccessControl;


// Unique index for the entity
[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class User : SoftDeletableEntity
{
    // Basic user information

    [Column(TypeName = "nvarchar(100)")]
    public string Username { get; set; }

    // The hashed password of the user by SecurePasswordHasher.
    [Column(TypeName = "nvarchar(100)")]
    public string Password { get; set; }

    [Column(TypeName = "nvarchar(150)")]
    public string Email { get; set; }

    // User profile details

    [Column(TypeName = "nvarchar(150)")]
    public string FullName { get; set; }

    [Column(TypeName = "date")]
    public DateTime? DateOfBirth { get; set; }

    [Column(TypeName = "nvarchar(20)")]
    public string PhoneNumber { get; set; }
    public UserStatus Status { get; set; } = UserStatus.IsActive;

    // =================================================
    // Navigation property - associated with this entity
    // =================================================
    public virtual ICollection<UserRole> UserRoles { get; set; }
}
