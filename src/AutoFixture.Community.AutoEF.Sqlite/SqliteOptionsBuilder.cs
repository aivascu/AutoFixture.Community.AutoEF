using AutoFixture.Kernel;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AutoFixture.Community.AutoEF.Sqlite
{
    /// <summary>
    /// Builds <see cref="DbContextOptions{TContext}"/> instances.
    /// </summary>
    public class SqliteOptionsBuilder : DbContextOptionsBuilder
    {
        public SqliteOptionsBuilder(IRequestSpecification optionsSpecification)
            : base(optionsSpecification)
        {
        }

        public SqliteOptionsBuilder()
            : base(new DbContextOptionsSpecification())
        {
        }

        /// <inheritdoc />
        protected override DbContextOptions<TContext> Build<TContext>(ISpecimenContext context)
        {
            var connection = context.Create<SqliteConnection>();
            return new DbContextOptionsBuilder<TContext>()
                .UseSqlite(connection)
                .Options;
        }
    }
}
