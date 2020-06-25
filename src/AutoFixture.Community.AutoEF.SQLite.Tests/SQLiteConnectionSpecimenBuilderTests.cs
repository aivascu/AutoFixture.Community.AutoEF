using System;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Moq;
using Xunit;

namespace AutoFixture.Community.AutoEF.SQLite.Tests
{
    public class SQLiteConnectionSpecimenBuilderTests
    {
        [Theory]
        [AutoDomainData]
        public void Create_ShouldThrowArgumentException_WhenSpecimenContextIsNull(
            SqliteConnectionSpecimenBuilder builder)
        {
            Action act = () => builder.Create(typeof(SqliteConnection), null);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnNoSpecimen_WhenRequestTypeNotSqliteConnection(
            Mock<ISpecimenContext> contextMock,
            SqliteConnectionSpecimenBuilder builder)
        {
            var noSpecimen = builder.Create(typeof(string), contextMock.Object);

            noSpecimen.Should().BeOfType<NoSpecimen>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnSqliteConnectionInstance_WhenRequestTypeIsSqliteConnection(
            Mock<ISpecimenContext> contextMock,
            SqliteConnectionSpecimenBuilder builder)
        {
            var noSpecimen = builder.Create(typeof(SqliteConnection), contextMock.Object);

            noSpecimen.Should().BeOfType<SqliteConnection>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldCreateSqliteConnection_WithConnectionString_WhenRequestTypeIsSqliteConnection(
            Mock<ISpecimenContext> contextMock,
            SqliteConnectionSpecimenBuilder builder)
        {
            var connection = builder.Create(typeof(SqliteConnection), contextMock.Object);

            connection.As<SqliteConnection>().ConnectionString.Should().Be("DataSource=:memory:");
        }

        [Theory]
        [AutoDomainData]
        public void Ctors_ShouldReceiveInitializedParameters(GuardClauseAssertion assertion)
        {
            var members = typeof(SqliteConnectionSpecimenBuilder).GetConstructors();

            assertion.Verify(members);
        }
    }
}
