using BaseRMS.DbContexts;
using BaseRMS.Entities;
using Microsoft.EntityFrameworkCore;

namespace BaseRMS.Repositories;

public class ContractRepository (ApplicationDbContext applicationDbContext)
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public IQueryable<Contract> GetContracts()
    {
        return _applicationDbContext.Contracts.AsNoTracking().AsQueryable();
    }

    public async Task CreateContractAsync(Contract contract)
    {
        await _applicationDbContext.Contracts.AddAsync(contract);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task UpdateContractAsync(Contract contract)
    {
        _applicationDbContext.Contracts.Update(contract);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task CreateContractTypeAsync(ContractTypes contractType)
    {
        await _applicationDbContext.ContractTypes.AddAsync(contractType);
        await _applicationDbContext.SaveChangesAsync();
    }
}
