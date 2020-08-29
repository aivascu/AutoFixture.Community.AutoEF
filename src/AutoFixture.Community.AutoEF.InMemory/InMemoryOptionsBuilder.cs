using System;
using Microsoft.EntityFrameworkCore;

namespace AutoFixture.Community.AutoEF.InMemory
{
    public class InMemoryOptionsBuilder : OptionsBuilder
    {
        public InMemoryOptionsBuilder(string databaseName)
        {
            this.DatabaseName = databaseName
                ?? throw new ArgumentNullException(nameof(databaseName));
        }

        public InMemoryOptionsBuilder()
            : this(Guid.NewGuid().ToString())
        {
        }

        public string DatabaseName { get; }

        public override DbContextOptions<TContext> Build<TContext>()
        {
            return new DbContextOptionsBuilder<TContext>()
                .UseInMemoryDatabase(this.DatabaseName)
                .Options;
        }
    }
}
