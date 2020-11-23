using System;
using AutoFixture.Kernel;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AutoFixture.Community.AutoEF.Sqlite
{
    /// <summary>
    /// Builds <see cref="DbContextOptions{TContext}"/> instances,
    /// using the SQLite database provider.
    /// </summary>
    public class SqliteConnectionBuilder : ISpecimenBuilder
    {
        public SqliteConnectionBuilder(
            string connectionString,
            IRequestSpecification connectionSpecification)
        {
            this.ConnectionString = connectionString
                ?? throw new ArgumentNullException(nameof(connectionString));
            this.ConnectionSpecification = connectionSpecification
                ?? throw new ArgumentNullException(nameof(connectionSpecification));
        }

        public SqliteConnectionBuilder(string connectionString)
            : this(connectionString, new SqliteConnectionSpecification())
        {
        }

        public SqliteConnectionBuilder()
            : this("DataSource=:memory:")
        {
        }

        /// <summary>
        /// Specifies the request for a <see cref="SqliteConnection"/>.
        /// </summary>
        public IRequestSpecification ConnectionSpecification { get; }

        /// <summary>
        /// The connection string used to create new <see cref="SqliteConnection"/> instances.
        /// </summary>
        public string ConnectionString { get; }

        /// <inheritdoc />
        public object Create(object request, ISpecimenContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!this.ConnectionSpecification.IsSatisfiedBy(request))
            {
                return new NoSpecimen();
            }

            return new SqliteConnection(this.ConnectionString);
        }
    }
}
