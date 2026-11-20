using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Results;
using RepairIT.Shared.Users;

namespace RepairIT.UserService.Services;

public class UserService(UserContext context) : IUserService
{
    public async Task<Result<IEnumerable<User>>> GetAllAsync()
    {
        var users = await context.Users.ToListAsync();
        return Result.Success<IEnumerable<User>>(users);
    }

    public async Task<Result<User>> GetByIdAsync(Guid id)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            return UserErrors.CouldNotFindUserById;
        }

        return user;
    }

    public async Task<Result<User>> AddAsync(User user)
    {
        var emailAlreadyUsed = await context.Users.AnyAsync(u => u.Email == user.Email);
        if (emailAlreadyUsed)
        {
            return UserErrors.EmailAlreadyUsed;
        }

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }
}
