using BaseRMS.Entities.AttatchmentClasses;

namespace BaseRMS.DTOs;

public class ClientCreateDTO
{
    public required string Address { get; set; }
    public required string Name { get; set; }
    public int PhoneNumber { get; set; }
    public string? Email { get; set; }
    public required IFormFile ClientImage { get; set; }
    public ICollection<ApplicationFileDTO>? Attachments { get; set; }
}
