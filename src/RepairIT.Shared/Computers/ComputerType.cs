using System.Text.Json.Serialization;

namespace RepairIT.Shared.Computers;

[JsonConverter(typeof(JsonStringEnumConverter<ComputerType>))]
public enum ComputerType
{
    Laptop,
    Desktop,
    Server,
}
