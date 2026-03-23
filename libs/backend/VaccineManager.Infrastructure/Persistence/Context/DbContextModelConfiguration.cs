using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VaccineManager.Domain.Entities;

namespace VaccineManager.Infrastructure.Persistence.Context;

public static class DbContextModelConfiguration
{
    public static void ApplySharedConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                continue;

            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var property = Expression.Property(parameter, nameof(BaseEntity.DeletedAt));
            var condition = Expression.Equal(property, Expression.Constant(null, typeof(DateTime?)));
            var lambda = Expression.Lambda(condition, parameter);
            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            modelBuilder.Entity(entityType.ClrType).Ignore(nameof(BaseEntity.IsDeleted));
        }
    }
}
