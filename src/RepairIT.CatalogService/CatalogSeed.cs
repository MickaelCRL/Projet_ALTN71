using RepairIT.Shared.Computers;

namespace RepairIT.CatalogService;

public static class CatalogSeed
{
    public static List<Computer> Computers() =>
        [
            new()
            {
                Brand = "Dell",
                Model = "XPS 13",
                Type = ComputerType.Laptop,
                Processor = "Intel Core i7-1360P",
                RamInGb = 16,
                StorageInGb = 512,
                Price = 1299m,
                Stock = 5,
            },
            new()
            {
                Brand = "Lenovo",
                Model = "ThinkPad T14",
                Type = ComputerType.Laptop,
                Processor = "AMD Ryzen 7 PRO 7840U",
                RamInGb = 32,
                StorageInGb = 1024,
                Price = 1499m,
                Stock = 3,
            },
            new()
            {
                Brand = "HP",
                Model = "EliteDesk 800 G9",
                Type = ComputerType.Desktop,
                Processor = "Intel Core i5-12500",
                RamInGb = 16,
                StorageInGb = 512,
                Price = 899m,
                Stock = 6,
            },
            new()
            {
                Brand = "Dell",
                Model = "PowerEdge T150",
                Type = ComputerType.Server,
                Processor = "Intel Xeon E-2314",
                RamInGb = 32,
                StorageInGb = 2000,
                Price = 1899m,
                Stock = 2,
            },
        ];
}
