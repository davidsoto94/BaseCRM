using BaseCRM.DbContexts;
using BaseCRM.Entities;
using Microsoft.EntityFrameworkCore;

namespace BaseCRM.Repositories;

public class AuthRepository(ApplicationDbContext applicationDbContext)
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public IQueryable<TrustedDevice> GetTrustedDevices()
    {
        return _applicationDbContext.TrustedDevices.AsNoTracking().AsQueryable();
    }
}
