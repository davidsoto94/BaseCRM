using BaseRMS.Entities.AttatchmentClasses;
using System.Text.Json.Serialization;

namespace BaseRMS.Entities;

public class Client
{
    public int Id { get; set; }
    public required string Address { get; set; }
    public required string Name { get; set; }
    public long PhoneNumber { get; set; }
    public string? Email { get; set; }
    [JsonIgnore]
    public string? ClientImagePath { get; set; }
    [JsonIgnore]
    public bool Active { get; set; } = true;
    public ICollection<ClientAttachment> Attachments { get; set; } = new List<ClientAttachment>();
    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();

}
