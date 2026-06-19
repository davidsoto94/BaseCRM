using BaseRMS.DbContexts;
using BaseRMS.Entities;
using Microsoft.EntityFrameworkCore;

namespace BaseRMS.Repositories;

public class EmployeeRepository (ApplicationDbContext applicationDbContext)
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public IQueryable<Employee> GetEmployees()
    {
        return _applicationDbContext.Employees.AsNoTracking().AsQueryable();
    }

    public async Task CreateEmployeeAsync(Employee employee)
    {
        await _applicationDbContext.Employees.AddAsync(employee);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        _applicationDbContext.Employees.Update(employee);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task CreateIdentificationTypeAsync(PersonalIdentificationType identificationType)
    {
        await _applicationDbContext.PersonalIdentificationTypes.AddAsync(identificationType);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task CreateEmployeeContractAsync(List<EmployeeContract> employeeContracts)
    {
        await _applicationDbContext.EmployeesContracts.AddRangeAsync(employeeContracts);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task UpdateEmployeeContractAsync(EmployeeContract employeeContract)
    {
        _applicationDbContext.EmployeesContracts.Update(employeeContract);
        await _applicationDbContext.SaveChangesAsync();
    }
}
