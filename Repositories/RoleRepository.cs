using BaseCRM.DbContexts;
using BaseCRM.Entities;
using Microsoft.EntityFrameworkCore;

namespace BaseCRM.Repositories;

public class RoleRepository(ApplicationDbContext applicationDbContext)
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public IQueryable<ApplicationRole> GetApplicationRoles()
    {
        return _applicationDbContext.ApplicationRoles.AsNoTracking().AsQueryable();
    }
}
