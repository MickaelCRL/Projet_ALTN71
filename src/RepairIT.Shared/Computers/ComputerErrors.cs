using RepairIT.Shared.Results;

namespace RepairIT.Shared.Computers;

public static class ComputerErrors
{
    public static readonly Error CouldNotFindComputerById = new(
        "CouldNotFindComputerById",
        "Could not find computer by id."
    );
}
