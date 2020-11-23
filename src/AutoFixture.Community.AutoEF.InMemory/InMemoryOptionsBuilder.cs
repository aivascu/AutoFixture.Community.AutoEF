using System;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace AutoFixture.Community.AutoEF.InMemory
{
    /// <summary>
    /// Builds <see cref="DbContextOptions{TContext}"/> instances using the in-memory databse provider.
    /// </summary>
    public class InMemoryOptionsBuilder : DbContextOptionsBuilder
    {
        public InMemoryOptionsBuilder(string databaseName, IRequestSpecification optionsSpecifications)
            : base(optionsSpecifications)
        {
            this.DatabaseName = databaseName
                ?? throw new ArgumentNullException(nameof(databaseName));
        }

        public InMemoryOptionsBuilder(string databaseName)
            : this(databaseName, new DbContextOptionsSpecification())
        {
        }

        /// <summary>
        /// The name of the in-memory database.
        /// </summary>
        public string DatabaseName { get; }

        /// <inheritdoc />
        protected override DbContextOptions<TContext> Build<TContext>(ISpecimenContext context)
        {
            return new DbContextOptionsBuilder<TContext>()
                .UseInMemoryDatabase(this.DatabaseName)
                .Options;
        }
    }
}
