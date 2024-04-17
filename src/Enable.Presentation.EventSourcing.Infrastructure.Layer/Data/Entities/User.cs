using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Entities;

/// <summary>
/// A user entity that represents a user in the system
/// </summary>
[PrimaryKey(nameof(Id))]
[Index(nameof(Email), IsUnique = true)]
[Table("Users", Schema = "dbo")]
public class User : IEntity
{
    [Key]
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(128)]
    [Required]
    public required string FirstName { get; set; }

    [MaxLength(128)]
    [Required]
    public required string LastName { get; set; }

    [EmailAddress]
    public required string Email { get; set; }

    [Phone]
    public required string PhoneNumber { get; set; }

    public DateTimeOffset LastModified { get; set; } = DateTimeOffset.UtcNow;
}
