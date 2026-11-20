using RepairIT.Shared.Users;

namespace RepairIT.UserService;

public static class UserSeed
{
    public static List<User> Users() =>
        [
            new()
            {
                LastName = "Martin",
                FirstName = "Alice",
                Email = "alice.martin@repairit.fr",
                Role = Role.Admin,
            },
            new()
            {
                LastName = "Bernard",
                FirstName = "Lucas",
                Email = "lucas.bernard@repairit.fr",
                Role = Role.Manager,
            },
            new()
            {
                LastName = "Dubois",
                FirstName = "Emma",
                Email = "emma.dubois@repairit.fr",
                Role = Role.Manager,
            },
        ];
}
