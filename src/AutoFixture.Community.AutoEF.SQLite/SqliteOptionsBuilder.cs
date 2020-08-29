using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AutoFixture.Community.AutoEF.SQLite
{
    public class SqliteOptionsBuilder : OptionsBuilder
    {
        public SqliteOptionsBuilder(SqliteConnection connection)
        {
            this.Connection = connection
                              ?? throw new ArgumentNullException(nameof(connection));
        }

        public SqliteConnection Connection { get; }

        public override DbContextOptions<TContext> Build<TContext>()
        {
            return new DbContextOptionsBuilder<TContext>()
                .UseSqlite(this.Connection)
                .Options;
        }
    }
}
