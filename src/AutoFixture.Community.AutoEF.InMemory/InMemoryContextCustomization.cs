using System;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace AutoFixture.Community.AutoEF.InMemory
{
    /// <summary>
    /// Customizes the fixture to create <see cref="DbContextOptions{TContext}"/> instances,
    /// using the in-memory database provider.
    /// </summary>
    public class InMemoryContextCustomization : ICustomization
    {
        public InMemoryContextCustomization(string databaseName)
        {
            this.DatabaseName = databaseName
                ?? throw new ArgumentNullException(nameof(databaseName));
        }

        public InMemoryContextCustomization()
            : this(Guid.NewGuid().ToString())
        {
        }

        /// <summary>
        /// The name of the in-memory database instances.
        /// </summary>
        public string DatabaseName { get; }

        /// <inheritdoc />
        public void Customize(IFixture fixture)
        {
            if (fixture is null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            fixture.Customizations.Add(new Omitter(new DbSetSpecification()));
            fixture.Customizations.Add(new InMemoryOptionsBuilder(this.DatabaseName));
        }
    }
}
