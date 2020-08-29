using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace AutoFixture.Community.AutoEF
{
    public abstract class OptionsBuilder : IOptionsBuilder
    {
        public virtual object Build(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!typeof(DbContext).IsAssignableFrom(type) || type.IsAbstract)
            {
                throw new ArgumentException(
                    $"The context type should be a non-abstract class inherited from {typeof(DbContext)}",
                    nameof(type));
            }

            return Array
                .Find(
                    this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance),
                    m => m.Name == nameof(Build) && m.IsGenericMethodDefinition)
                .MakeGenericMethod(type)
                .Invoke(this, Array.Empty<object>());
        }

        public abstract DbContextOptions<TContext> Build<TContext>() where TContext : DbContext;
    }
}
