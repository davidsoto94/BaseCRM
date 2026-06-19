using BaseRMS.DbContexts;
using BaseRMS.Entities;
using Microsoft.EntityFrameworkCore;

namespace BaseRMS.Repositories;

public class EventRepository (ApplicationDbContext applicationDbContext)
{
    public IQueryable<Event> GetEvents()
    {
        return applicationDbContext.Events.AsNoTracking();
    }

    public IQueryable<EventCategory> GetEventCategories()
    {
        return applicationDbContext.EventCategories.AsNoTracking();
    }

    public async Task CreateEventAsync(Event eventEntity)
    {
        await applicationDbContext.Events.AddAsync(eventEntity);
        await applicationDbContext.SaveChangesAsync();
    }

    public async Task CreateEventCategoryAsync(EventCategory eventCategory)
    {
        await applicationDbContext.EventCategories.AddAsync(eventCategory);
        await applicationDbContext.SaveChangesAsync();
    }
    public async Task UpdateEventAsync(Event eventEntity)
    {
        applicationDbContext.Events.Update(eventEntity);
        await applicationDbContext.SaveChangesAsync();
    }
    public async Task UpdateEventCategoryAsync(EventCategory eventCategory)
    {
        applicationDbContext.EventCategories.Update(eventCategory);
        await applicationDbContext.SaveChangesAsync();
    }

}
