using BaseRMS.DbContexts;
using BaseRMS.Entities;
using Microsoft.EntityFrameworkCore;

namespace BaseRMS.Repositories;

public class ClientRepository (ApplicationDbContext applicationDbContext)
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public IQueryable<Client> GetClients()
    {
        return _applicationDbContext.Clients.AsNoTracking().AsQueryable();
    }

    public async Task CreateClientAsync(Client client)
    {
        await _applicationDbContext.Clients.AddAsync(client);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task UpdateClientAsync(Client client)
    {
        _applicationDbContext.Clients.Update(client);
        await _applicationDbContext.SaveChangesAsync();
    }
}
