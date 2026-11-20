using System.ComponentModel.DataAnnotations;

namespace RepairIT.Shared.Users;

public class User
{
    public Guid Id { get; init; } = Guid.NewGuid();

    // keycloak arrive en v5, en attendant on met une valeur par defaut
    public Guid KeycloakUserId { get; set; } = Guid.Empty;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    public Role Role { get; set; } = Role.Manager;
}
