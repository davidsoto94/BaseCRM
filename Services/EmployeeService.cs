using BaseRMS.Entities;
using BaseRMS.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BaseRMS.Services;

public class EmployeeService (EmployeeRepository employeeRepository)
{
    private readonly EmployeeRepository _employeeRepository = employeeRepository;

    public async Task GetEmployeesWithContracts()
    {
        var employees = await _employeeRepository.GetEmployees()
            .Include(e => e.PersonalIdentificationType)
            .Include(e => e.Contracts)
            .ThenInclude(ec => ec.Contract)
            .Where(e => e.Contracts.Any())
            .ToListAsync();
    }

    public async Task AddEmployeeToContract(EmployeeContract employeeContract)
    {
        await _employeeRepository.CreateEmployeeContractAsync(new List<EmployeeContract> { employeeContract });
    }

    public async Task CreateEmployee(Employee employee)
    {
        await _employeeRepository.CreateEmployeeAsync(employee);
    }

    public async Task UpdateEmployee(Employee employee)
    {
        await _employeeRepository.UpdateEmployeeAsync(employee);
    }

    public async Task CreateIdentificationType(PersonalIdentificationType identificationType)
    {
        await _employeeRepository.CreateIdentificationTypeAsync(identificationType);
    }

    public async Task RemoveEmployeeFromContract(int employeeId, int contractId, DateOnly endDate)
    {
        var employeeContract = await _employeeRepository.GetEmployees()
            .Where(e => e.Id == employeeId)
            .SelectMany(e => e.Contracts)
            .FirstOrDefaultAsync(ec => ec.ContractId == contractId);

        if (employeeContract == null) {
            return;
        }

        employeeContract.EndDate = endDate.ToDateTime(TimeOnly.MinValue);
        await _employeeRepository.UpdateEmployeeContractAsync(employeeContract);

    }
}
