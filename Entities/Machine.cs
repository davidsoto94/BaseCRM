namespace BaseRMS.Entities;

public class Machine
{
    public int Id { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public int MachineTypeId { get; set; }
    public MachineType MachineType { get; set; } = null!;
}
