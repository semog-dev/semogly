using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;
using Semogly.Core.Domain.SharedContext.Entities;
using System.Reflection;

namespace Semogly.Core.Infrastructure.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyBaseEntityConfiguration(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model
            .GetEntityTypes()
            .Where(t => IsSubclassOfRawGeneric(typeof(Entity<>), t.ClrType)))
        {
            var entityMethod = typeof(ModelBuilder)
                .GetMethods()
                .First(m =>
                    m.Name == "Entity" &&
                    m.IsGenericMethod &&
                    m.GetParameters().Length == 0);

            var genericEntityMethod = entityMethod.MakeGenericMethod(entityType.ClrType);
            dynamic entity = genericEntityMethod.Invoke(modelBuilder, null)!;

            // PublicId unique

            var publicIdProperty = entityType.FindProperty("PublicId");

            publicIdProperty?.SetColumnName("public_id");

            entity?.HasIndex("PublicId").IsUnique();

            var trackerProperty = entityType.ClrType.GetProperty(
                "Tracker",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );

            if (trackerProperty == null)
                continue;

            var trackerType = trackerProperty.PropertyType;

            var param = Expression.Parameter(entityType.ClrType, "e");
            var propertyAccess = Expression.Property(param, trackerProperty);
            var lambda = Expression.Lambda(propertyAccess, param);

            // Método genérico OwnsOne<TEntity, TProperty>
            var ownsOneMethod = typeof(EntityTypeBuilder<>)
                .MakeGenericType(entityType.ClrType)
                .GetMethods()
                .Where(m => m.Name == "OwnsOne")
                .FirstOrDefault(m =>
                {
                    var p = m.GetParameters();
                    return p.Length == 2
                        && p[0].ParameterType.IsGenericType
                        && p[0].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>)
                        && p[1].ParameterType.IsGenericType
                        && p[1].ParameterType.GetGenericTypeDefinition() == typeof(Action<>);
                }) ?? throw new InvalidOperationException(
                    $"OwnsOne overload not found for entity {entityType.ClrType.Name}");
            var genericOwnsOne = ownsOneMethod.MakeGenericMethod(trackerType);

                        var ownedBuilderType = typeof(OwnedNavigationBuilder<,>)
                .MakeGenericType(entityType.ClrType, trackerType);

            var actionType = typeof(Action<>).MakeGenericType(ownedBuilderType);

            var configureMethod = typeof(ModelBuilderExtensions)
                .GetMethod(nameof(ConfigureTracker), BindingFlags.Static | BindingFlags.NonPublic)!
                .MakeGenericMethod(entityType.ClrType, trackerType);

            var action = Delegate.CreateDelegate(actionType, configureMethod);

            genericOwnsOne.Invoke(entity, new object[]
            {
                lambda,
                action
            });

        }
    }

    private static void ConfigureShadowProperty<TEntity, TProperty>(
        OwnedNavigationBuilder<TEntity, TProperty> owned,
        string clrName,
        string columnName,
        bool required)
        where TEntity : class
        where TProperty : class
    {
        owned.Property(clrName)
            .HasColumnName(columnName)
            .HasColumnType("timestamp with time zone")
            .IsRequired(required);
    }

    private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
    {
        while (toCheck != null && toCheck != typeof(object))
        {
            var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (cur == generic)
                return true;

            toCheck = toCheck.BaseType!;
        }
        return false;
    }

    private static void ConfigureTracker<TEntity, TTracker>(
        OwnedNavigationBuilder<TEntity, TTracker> owned)
        where TEntity : class
        where TTracker : class
    {
        ConfigureShadowProperty(owned, "CreatedAtUtc", "created_at", true);
        ConfigureShadowProperty(owned, "UpdatedAtUtc", "updated_at", false);
        owned.WithOwner();
    }
}

