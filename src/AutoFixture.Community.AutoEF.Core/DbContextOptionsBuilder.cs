using System;
using System.Reflection;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace AutoFixture.Community.AutoEF
{
    public abstract class DbContextOptionsBuilder : ISpecimenBuilder
    {
        protected DbContextOptionsBuilder(IRequestSpecification optionsSpecification)
        {
            this.OptionsSpecification = optionsSpecification
                ?? throw new ArgumentNullException(nameof(optionsSpecification));
        }

        /// <summary>
        /// Describes the request for a <see cref="DbContextOptions{TContext}"/> instance.
        /// </summary>
        public IRequestSpecification OptionsSpecification { get; protected set; }

        /// <inheritdoc />
        public virtual object Create(object request, ISpecimenContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!this.OptionsSpecification.IsSatisfiedBy(request))
            {
                return new NoSpecimen();
            }

            return this.GetContextType(request) switch
            {
                Type type => this.Build(type, context),
                _ => new NoSpecimen(),
            };
        }

        /// <summary>
        /// Gets the concrete context type from the request.
        /// </summary>
        /// <param name="request">The request instance.</param>
        /// <returns>Returns the concrete context type.</returns>
        protected virtual Type GetContextType(object request)
            => request switch
            {
                Type type => type.GetGenericArguments()[0],
                _ => null,
            };

        /// <summary>
        /// Builds <see cref="DbContextOptions{TContext}"/> instances, using the protected generic <see cref="Build{TContext}(ISpecimenContext)"/> method.
        /// </summary>
        /// <param name="type">The <see cref="DbContext"/> type.</param>
        /// <param name="context">The specimen creation context.</param>
        /// <returns>Returns a <see cref="DbContextOptions{TContext}" /> instance, casted to an <see cref="object"/>.</returns>
        protected virtual object Build(Type type, ISpecimenContext context)
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
                    this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic),
                    m => m.Name == nameof(Build) && m.IsGenericMethodDefinition)
                .MakeGenericMethod(type)
                .Invoke(this, new[] { context });
        }

        /// <summary>
        /// Builds <see cref="DbContextOptions{TContext}"/> instances.
        /// </summary>
        /// <typeparam name="TContext">The concrete <see cref="DbContext"/> context type.</typeparam>
        /// <param name="context">The specimen creation context.</param>
        /// <returns>Returns a <see cref="DbContextOptions{TContext}"/> instance of type.</returns>
        protected abstract DbContextOptions<TContext> Build<TContext>(ISpecimenContext context)
            where TContext : DbContext;
    }
}
