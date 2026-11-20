using RepairIT.Shared.Results;
using RepairIT.Shared.Users;

namespace RepairIT.UserService.Services;

public interface IUserService
{
    Task<Result<IEnumerable<User>>> GetAllAsync();
    Task<Result<User>> GetByIdAsync(Guid id);
    Task<Result<User>> AddAsync(User user);
}
