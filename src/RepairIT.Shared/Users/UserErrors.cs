using RepairIT.Shared.Results;

namespace RepairIT.Shared.Users;

public static class UserErrors
{
    public static readonly Error CouldNotFindUserById = new(
        "CouldNotFindUserById",
        "Could not find user by id."
    );

    public static readonly Error EmailAlreadyUsed = new(
        "EmailAlreadyUsed",
        "A user with this email already exists."
    );
}
