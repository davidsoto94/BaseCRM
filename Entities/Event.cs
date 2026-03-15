using BaseRMS.Entities.AttatchmentClasses;

namespace BaseRMS.Entities;

public class Event
{
    public int Id { get; set; }
    public int ContractId { get; set; }
    public Contract Contract { get; set; } = null!;
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public int? MachineId { get; set; }
    public Machine? Machine { get; set; }
    public required string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Observations { get; set; } = string.Empty;
    public double Cost { get; set; }
    public ICollection<EventAttachment> Attachments { get; set; } = new List<EventAttachment>();
}
