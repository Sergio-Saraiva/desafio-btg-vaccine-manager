using Microsoft.Data.Sqlite;

namespace VaccineManager.Infrastructure.Persistence;

public static class DatabaseSync
{
    public static void SyncWriteToRead(string writeConnectionString, string readConnectionString)
    {
        using var source = new SqliteConnection(writeConnectionString);
        using var destination = new SqliteConnection(readConnectionString);
        source.Open();
        destination.Open();
        source.BackupDatabase(destination);
    }
}
