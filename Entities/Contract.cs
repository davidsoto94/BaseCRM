using BaseRMS.Entities.AttatchmentClasses;

namespace BaseRMS.Entities;

public class Contract
{
    public int Id { get; set; }
    public string Address { get; set; } = "";
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
    public required string Name { get; set; }
    public string? ContractId { get; set; }
    public int ContractTypeId { get; set; }
    public ContractTypes ContractType { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    /// <summary>
    /// Value paid for the whole contract, if the contract is paid by the hour, this value will be null and the HourAmmount property will be used instead.
    /// </summary>
    public decimal? ContractAmount { get; set; }
    /// <summary>
    /// The value paid for each hour of work, if the contract is paid by the hour, this value will be used and the ContractAmount property will be null.
    /// </summary>
    public decimal? HourAmmount { get; set; }
    public ICollection<ContractAttachment> ContractAttachments { get; set; } = new List<ContractAttachment>();
}
