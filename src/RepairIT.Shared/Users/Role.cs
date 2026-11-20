using System.Text.Json.Serialization;

namespace RepairIT.Shared.Users;

[JsonConverter(typeof(JsonStringEnumConverter<Role>))]
public enum Role
{
    Manager,
    Admin,
}
