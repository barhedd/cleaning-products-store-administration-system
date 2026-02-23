using CleaningProductsStore.Application.DataTransferObjects;
using CleaningProductsStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CleaningProductsStore.Infrastructure.Contexts;

public class CleaningProductsStoreContext(
    DbContextOptions<CleaningProductsStoreContext> options) : DbContext(options)
{
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductByStatusDto>().HasNoKey();
        base.OnModelCreating(modelBuilder);

        // Aplicar configuraciones
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CleaningProductsStoreContext).Assembly);

        // Aplicar filtro global de Soft Delete
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(GetIsDeletedRestriction(entityType.ClrType));
            }
        }
    }

    private static LambdaExpression GetIsDeletedRestriction(Type type)
    {
        var parameter = Expression.Parameter(type, "e");
        var prop = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
        var condition = Expression.Equal(prop, Expression.Constant(false));
        return Expression.Lambda(condition, parameter);
    }

    public override async Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            var now = DateTimeOffset.UtcNow;

            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = now;
                    entry.Entity.CreatedBy = "SYSTEM";
                    break;

                case EntityState.Modified:
                    entry.Entity.EditedDate = now;
                    entry.Entity.EditedBy = "SYSTEM";
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedDate = now;
                    entry.Entity.DeletedBy = "SYSTEM";
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
