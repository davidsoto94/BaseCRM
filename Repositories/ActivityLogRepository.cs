using BaseRMS.DbContexts;
using BaseRMS.Entities;
using Microsoft.EntityFrameworkCore;

namespace BaseRMS.Repositories;

public class ActivityLogRepository(ApplicationDbContext applicationDbContext)
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public IQueryable<ActivityLog> GetActivityLogs()
    {
        return _applicationDbContext.ActivityLogs.AsNoTracking().AsQueryable();
    }

    public async Task AddActivityLogAsync(ActivityLog ActivityLog)
    {
        _applicationDbContext.ActivityLogs.Add(ActivityLog);
        await _applicationDbContext.SaveChangesAsync();
    }
}
