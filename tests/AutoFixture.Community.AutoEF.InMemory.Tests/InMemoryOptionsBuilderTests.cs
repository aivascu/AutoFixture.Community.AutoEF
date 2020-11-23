using System;
using AutoFixture.Kernel;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.Infrastructure.Internal;
using Moq;
using TestClasses;
using Xunit;

namespace AutoFixture.Community.AutoEF.InMemory.Tests
{
    public class InMemoryOptionsBuilderTests
    {
        [Fact]
        public void ThrowsNullArgumentExceptionForNullSpecimenContext()
        {
            var databaseName = Guid.NewGuid().ToString();
            var builder = new InMemoryOptionsBuilder(databaseName);

            Action act = () => builder.Create(typeof(DbContextOptions<TestDbContext>), null);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData(typeof(DbSet<>))]
        [InlineData(typeof(DbContext))]
        [InlineData("NoT A vAlID rEQueSt")]
        [InlineData(3.14f)]
        public void ReturnsNoSpecimenForNonOptionsRequest(object request)
        {
            var databaseName = Guid.NewGuid().ToString();
            var contextMock = new Mock<ISpecimenContext>();
            var builder = new InMemoryOptionsBuilder(databaseName);

            var actual = builder.Create(request, contextMock.Object);

            actual.Should().BeOfType<NoSpecimen>();
        }

        [Fact]
        public void ReturnsRequestedDbContextOptionsInstance()
        {
            var databaseName = Guid.NewGuid().ToString();
            var contextMock = new Mock<ISpecimenContext>();
            var builder = new InMemoryOptionsBuilder(databaseName);

            var actual = builder.Create(typeof(DbContextOptions<TestDbContext>), contextMock.Object);

            actual.Should().NotBeNull()
                .And.Subject.Should().BeOfType<DbContextOptions<TestDbContext>>();
        }

        [Fact]
        public void ReturnsOptionsInstanceWithExpectedProvider()
        {
            var databaseName = Guid.NewGuid().ToString();
            var contextMock = new Mock<ISpecimenContext>();
            var builder = new InMemoryOptionsBuilder(databaseName);

            var actual = builder.Create(typeof(DbContextOptions<TestDbContext>), contextMock.Object);

            actual.As<DbContextOptions<TestDbContext>>()
                .Extensions.Should().Contain(x => x.GetType() == typeof(InMemoryOptionsExtension));
        }
    }
}
