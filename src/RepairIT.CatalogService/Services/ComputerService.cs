using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Computers;
using RepairIT.Shared.Results;

namespace RepairIT.CatalogService.Services;

public class ComputerService(CatalogContext context) : IComputerService
{
    public async Task<Result<IEnumerable<Computer>>> GetAllAsync()
    {
        var computers = await context.Computers.ToListAsync();
        return Result.Success<IEnumerable<Computer>>(computers);
    }

    public async Task<Result<Computer>> GetByIdAsync(Guid id)
    {
        var computer = await context.Computers.FirstOrDefaultAsync(c => c.Id == id);
        if (computer == null)
        {
            return ComputerErrors.CouldNotFindComputerById;
        }

        return computer;
    }

    public async Task<Result<Computer>> AddAsync(Computer computer)
    {
        context.Computers.Add(computer);
        await context.SaveChangesAsync();

        return computer;
    }

    public async Task<Result<Computer>> UpdateAsync(Guid id, Computer updatedComputer)
    {
        var existing = await context.Computers.FirstOrDefaultAsync(c => c.Id == id);
        if (existing == null)
        {
            return ComputerErrors.CouldNotFindComputerById;
        }

        existing.UpdateDetails(updatedComputer);
        await context.SaveChangesAsync();

        return existing;
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var computer = await context.Computers.FirstOrDefaultAsync(c => c.Id == id);
        if (computer == null)
        {
            return Result.Failure(ComputerErrors.CouldNotFindComputerById);
        }

        context.Computers.Remove(computer);
        await context.SaveChangesAsync();

        return Result.Success();
    }
}
