using Microsoft.Extensions.Options;
using VaccineManager.Application.Common.Settings;
using VaccineManager.Domain.Repositories;
using VaccineManager.Infrastructure.Persistence.Context;

namespace VaccineManager.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly WriteDbContext _dbContext;
    private readonly string _writeConnectionString;
    private readonly string _readConnectionString;

    public UnitOfWork(WriteDbContext dbContext, IOptions<AppSettings> appSettings)
    {
        _dbContext = dbContext;
        _writeConnectionString = appSettings.Value.SQLiteSettings.ConnectionString;
        _readConnectionString = appSettings.Value.SQLiteSettings.ReadConnectionString;
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.SaveChangesAsync(cancellationToken);
        DatabaseSync.SyncWriteToRead(_writeConnectionString, _readConnectionString);
        return result;
    }
}
