
namespace BaseRMS.Entities;

public class MachineType
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = "";
}
