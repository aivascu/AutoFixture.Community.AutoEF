using System;
using System.Linq;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal;
using TestClasses;
using Xunit;

namespace AutoFixture.Community.AutoEF.SQLite.Tests
{
    public class SQLiteOptionsBuilderTests
    {
        [Theory]
        [AutoData]
        public void Build_ShouldBuildDbContextOptionsInstance(
            Fixture fixture,
            SqliteConnectionSpecimenBuilder specimenBuilder)
        {
            fixture.Customizations.Add(specimenBuilder);

            using var connection = fixture.Freeze<SqliteConnection>();
            var builder = fixture.Create<SqliteOptionsBuilder>();

            var options = builder.Build(typeof(TestDbContext));

            options.Should().BeOfType<DbContextOptions<TestDbContext>>();
        }

        [Theory]
        [AutoData]
        public void Build_ShouldThrowArgumentNullException_WhenTypeIsNull(
            Fixture fixture,
            SqliteConnectionSpecimenBuilder specimenBuilder)
        {
            fixture.Customizations.Add(specimenBuilder);

            using var connection = fixture.Freeze<SqliteConnection>();
            var builder = fixture.Create<SqliteOptionsBuilder>();

            Action action = () => builder.Build(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [AutoData]
        public void Build_ShouldThrowArgumentNullException_WhenTypeIsNotDbContext(
            Fixture fixture,
            SqliteConnectionSpecimenBuilder specimenBuilder)
        {
            fixture.Customizations.Add(specimenBuilder);

            using var connection = fixture.Freeze<SqliteConnection>();
            var builder = fixture.Create<SqliteOptionsBuilder>();

            Action action = () => builder.Build(typeof(string));

            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [AutoData]
        public void Build_ShouldThrowArgumentNullException_WhenTypeIsAbstract(
            Fixture fixture,
            SqliteConnectionSpecimenBuilder specimenBuilder)
        {
            fixture.Customizations.Add(specimenBuilder);

            using var connection = fixture.Freeze<SqliteConnection>();
            var builder = fixture.Create<SqliteOptionsBuilder>();

            Action action = () => builder.Build(typeof(AbstractDbContext));

            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [AutoData]
        public void Constructors_ShouldInitializeProperties(ConstructorInitializedMemberAssertion assertion)
        {
            var members = typeof(SqliteOptionsBuilder).GetConstructors();

            assertion.Verify(members);
        }

        [Theory]
        [AutoData]
        public void Constructors_ShouldReceiveInitializedParameters(Fixture fixture, GuardClauseAssertion assertion)
        {
            fixture.Inject(new SqliteConnection("Data Source=:memory:"));
            var members = typeof(SqliteOptionsBuilder).GetConstructors();

            assertion.Verify(members);
        }

        [Fact]
        public void GenericBuild_ShouldCreateDbContextOptions_WithSqliteExtension()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            var context = new SqliteOptionsBuilder(connection)
                .Build<TestDbContext>();

            context.Extensions.Should().Contain(x => x.GetType() == typeof(SqliteOptionsExtension));
        }

        [Fact]
        public void GenericBuild_ShouldCreateDbContextOptions_WithSqliteExtension_WithConnectionString()
        {
            const string connectionString = "Data Source=:memory:";
            var connection = new SqliteConnection(connectionString);
            var extension = new SqliteOptionsBuilder(connection)
                .Build<TestDbContext>()
                .Extensions
                .Single(x => x.GetType() == typeof(SqliteOptionsExtension))
                .As<SqliteOptionsExtension>();

            extension.Connection.ConnectionString.Should().Be(connectionString);
        }

        [Fact]
        public void Build_ShouldCreateDbContextOptions_WithSqliteExtension()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            var context = new SqliteOptionsBuilder(connection)
                .Build(typeof(TestDbContext))
                .As<DbContextOptions<TestDbContext>>();

            context.Extensions.Should().Contain(x => x.GetType() == typeof(SqliteOptionsExtension));
        }

        [Fact]
        public void Build_ShouldCreateDbContextOptions_WithSqliteExtension_WithConnectionString()
        {
            const string connectionString = "Data Source=:memory:";
            var connection = new SqliteConnection(connectionString);
            var extension = new SqliteOptionsBuilder(connection)
                .Build(typeof(TestDbContext))
                .As<DbContextOptions<TestDbContext>>()
                .Extensions
                .Single(x => x.GetType() == typeof(SqliteOptionsExtension))
                .As<SqliteOptionsExtension>();

            extension.Connection.ConnectionString.Should().Be(connectionString);
        }

        private abstract class AbstractDbContext : DbContext { }
    }
}
