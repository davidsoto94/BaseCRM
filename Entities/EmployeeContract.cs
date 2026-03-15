using Microsoft.EntityFrameworkCore;

namespace BaseRMS.Entities;

[PrimaryKey(nameof(EmployeeId), nameof(ContractId))]

public class EmployeeContract
{
    public required int EmployeeId { get; set; }
    public required int ContractId { get; set; }
    /// <summary>
    /// This value is used to store the rate for the employee in this contract. It can be used for hourly, daily, or contract rate depending on the type of contract. The actual type of rate is determined by the contract type and the values that are filled in.
    /// For example, if the contract type is hourly, then the HourlyRate value will be used and the DailyRate and ContractRate will be ignored. 
    /// If the contract type is daily, then the DailyRate value will be used and the HourlyRate and ContractRate will be ignored. If the contract type is contract, then the ContractRate value will be used and the HourlyRate and DailyRate will be ignored.
    /// </summary>
    public decimal? HourlyRate { get; set; }
    /// <summary>
    /// This value is used to store the rate for the employee in this contract. It can be used for hourly, daily, or contract rate depending on the type of contract. The actual type of rate is determined by the contract type and the values that are filled in.
    /// For example, if the contract type is hourly, then the HourlyRate value will be used and the DailyRate and ContractRate will be ignored. 
    /// If the contract type is daily, then the DailyRate value will be used and the HourlyRate and ContractRate will be ignored. If the contract type is contract, then the ContractRate value will be used and the HourlyRate and DailyRate will be ignored.
    /// </summary>
    public decimal? DailyRate { get; set; }
    /// <summary>
    /// This value is used to store the rate for the employee in this contract. It can be used for hourly, daily, or contract rate depending on the type of contract. The actual type of rate is determined by the contract type and the values that are filled in.
    /// For example, if the contract type is hourly, then the HourlyRate value will be used and the DailyRate and ContractRate will be ignored. 
    /// If the contract type is daily, then the DailyRate value will be used and the HourlyRate and ContractRate will be ignored. If the contract type is contract, then the ContractRate value will be used and the HourlyRate and DailyRate will be ignored.
    /// </summary>
    public decimal? ContractRate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
