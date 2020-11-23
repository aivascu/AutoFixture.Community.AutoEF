using System;
using AutoFixture.Kernel;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AutoFixture.Community.AutoEF.Sqlite
{
    /// <summary>
    /// Customizes a fixture to create <see cref="DbContextOptions{TContext}"/> instances,
    /// using the SQLite database provider.
    /// </summary>
    public class SqliteContextCustomization : ICustomization
    {
        public SqliteContextCustomization(string connectionString)
        {
            this.ConnectionString = connectionString
                ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public SqliteContextCustomization()
            : this("DataSource=:memory:")
        {
        }

        /// <summary>
        /// The connection string used for creating the <see cref="SqliteConnection"/>.<br />
        /// Defaults to in-memory connection string.
        /// </summary>
        public string ConnectionString { get; }

        /// <inheritdoc />
        public void Customize(IFixture fixture)
        {
            if (fixture is null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            fixture.Customizations.Add(new Omitter(new DbSetSpecification()));
            fixture.Customizations.Add(new SqliteConnectionBuilder(this.ConnectionString));
            fixture.Customizations.Add(new SqliteOptionsBuilder());
        }
    }
}
