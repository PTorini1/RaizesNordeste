using Microsoft.EntityFrameworkCore;
using RaizesNordeste.Domain.Interfaces.Services;

namespace RaizesNordeste.Infrastructure.Persistence;

public class DataStoreInitializerService(RaizesNordesteDbContext dbContext) : IDataStoreInitializerService
{
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}
