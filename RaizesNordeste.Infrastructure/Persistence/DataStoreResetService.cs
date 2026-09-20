using Microsoft.EntityFrameworkCore;
using RaizesNordeste.Domain.Interfaces.Services;
using StackExchange.Redis;

namespace RaizesNordeste.Infrastructure.Persistence;

public class DataStoreResetService(
    RaizesNordesteDbContext dbContext,
    IConnectionMultiplexer connectionMultiplexer)
    : IDataStoreResetService
{
    public async Task ResetAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.EnsureDeletedAsync(cancellationToken);
        await dbContext.Database.MigrateAsync(cancellationToken);

        try
        {
            foreach (var endpoint in connectionMultiplexer.GetEndPoints())
            {
                var server = connectionMultiplexer.GetServer(endpoint);

                if (server.IsConnected)
                {
                    await server.FlushDatabaseAsync();
                }
            }
        }
        catch
        {
        }
    }
}
