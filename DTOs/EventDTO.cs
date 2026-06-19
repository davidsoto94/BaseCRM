
namespace BaseRMS.DTOs;

public class EventDTO
{
    public int EventCategoryId { get; set; }
    public int UserId { get; set; }
    public int ContractId { get; set; }
    public int? EmployeeId { get; set; } = null;
    public int? MachineId { get; set; } = null;
    public required string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Observations { get; set; } = string.Empty;
    public double Cost { get; set; }
    public ICollection<ApplicationFileDTO>? Attachments { get; set; }
}
