using System.ComponentModel.DataAnnotations;

namespace RepairIT.Shared.Computers;

public class Computer
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required, MaxLength(50)]
    public string Brand { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Model { get; set; } = string.Empty;

    public ComputerType Type { get; set; }

    [Required, MaxLength(100)]
    public string Processor { get; set; } = string.Empty;

    public int RamInGb { get; set; }
    public int StorageInGb { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public void UpdateDetails(Computer computer)
    {
        Brand = computer.Brand;
        Model = computer.Model;
        Type = computer.Type;
        Processor = computer.Processor;
        RamInGb = computer.RamInGb;
        StorageInGb = computer.StorageInGb;
        Price = computer.Price;
        Stock = computer.Stock;
    }
}
