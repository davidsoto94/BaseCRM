using BaseRMS.Entities.AttatchmentClasses;

namespace BaseRMS.Entities;

public class Employee
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string Address { get; set; } = string.Empty;
    public string? PersonalIdentificationString { get; set; }
    public int PersonalIdentificationTypeId { get; set; }
    public PersonalIdentificationType? PersonalIdentificationType { get; set; }
    public string? PicturePath { get; set; }
    public string? PhoneNumber { get; set; }
    public string EmergencyContactName { get; set; } = string.Empty;
    public string EmergencyContactPhone { get; set; } = string.Empty;
    public bool HasKey { get; set; } = false;
    public ICollection<EmployeeAttachment> Attachments { get; set; } = new List<EmployeeAttachment>();
    public ICollection<EmployeeContract> Contracts { get; set; } = new List<EmployeeContract>();
}
