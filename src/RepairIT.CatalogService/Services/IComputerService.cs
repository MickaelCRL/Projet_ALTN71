using RepairIT.Shared.Computers;
using RepairIT.Shared.Results;

namespace RepairIT.CatalogService.Services;

public interface IComputerService
{
    Task<Result<IEnumerable<Computer>>> GetAllAsync();
    Task<Result<Computer>> GetByIdAsync(Guid id);
    Task<Result<Computer>> AddAsync(Computer computer);
    Task<Result<Computer>> UpdateAsync(Guid id, Computer updatedComputer);
    Task<Result> DeleteAsync(Guid id);
}
