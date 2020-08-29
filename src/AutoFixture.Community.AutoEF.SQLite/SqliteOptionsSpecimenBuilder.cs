using System;
using AutoFixture.Kernel;
using Microsoft.Data.Sqlite;

namespace AutoFixture.Community.AutoEF.SQLite
{
    public class SqliteOptionsSpecimenBuilder : ISpecimenBuilder
    {
        public SqliteOptionsSpecimenBuilder(IRequestSpecification optionsBuilderSpecification)
        {
            this.OptionsBuilderSpecification = optionsBuilderSpecification
                ?? throw new ArgumentNullException(nameof(optionsBuilderSpecification));
        }

        public SqliteOptionsSpecimenBuilder()
            : this(new OptionsBuilderSpecification())
        {
        }

        public IRequestSpecification OptionsBuilderSpecification { get; }

        public object Create(object request, ISpecimenContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!this.OptionsBuilderSpecification.IsSatisfiedBy(request))
            {
                return new NoSpecimen();
            }

            var sqliteConnectionObj = context.Resolve(typeof(SqliteConnection));

            return sqliteConnectionObj is SqliteConnection sqliteConnection
                ? (object)new SqliteOptionsBuilder(sqliteConnection)
                : new NoSpecimen();
        }
    }
}
