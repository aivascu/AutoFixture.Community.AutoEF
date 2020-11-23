using System;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using TestClasses;
using Xunit;

namespace AutoFixture.Community.AutoEF.Core.Tests
{
    public class OptionsSpecimenBuilderTests
    {
        [Theory]
        [AutoDomainData]
        public void ShouldCreateDbContext(
            Mock<ISpecimenContext> contextMock,
            [Frozen] Mock<IRequestSpecification> optionsSpecification)
        {
            var optionsType = typeof(DbContextOptions<TestDbContext>);
            var options = new DbContextOptions<TestDbContext>();
            var builder = new DelegatingDbContextOptionsBuilder(optionsSpecification.Object)
            {
                BuildOptions = _ => options
            };
            optionsSpecification
                .Setup(x => x.IsSatisfiedBy(It.Is<Type>(x => x == optionsType)))
                .Returns(true);

            var actual = builder.Create(typeof(DbContextOptions<TestDbContext>), contextMock.Object);

            actual.Should().NotBeNull()
                .And.Subject.Should().BeOfType(optionsType);
        }

        [Theory]
        [AutoDomainData]
        public void ShouldReturnNoSpecimenForNullSpecimenContext(
            [Frozen] Mock<IRequestSpecification> optionsSpecification)
        {
            var optionsType = typeof(DbContextOptions<TestDbContext>);
            var options = new DbContextOptions<TestDbContext>();
            var builder = new DelegatingDbContextOptionsBuilder(optionsSpecification.Object)
            {
                BuildOptions = _ => options
            };
            optionsSpecification
                .Setup(x => x.IsSatisfiedBy(It.Is<Type>(x => x == optionsType)))
                .Returns(true);

            Action act = () => builder.Create(typeof(DbContextOptions<TestDbContext>), default);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineAutoDomainData(typeof(TestDbContext))]
        [InlineAutoDomainData(typeof(DbContext))]
        [InlineAutoDomainData(typeof(DbSet<>))]
        [InlineAutoDomainData(typeof(string))]
        [InlineAutoDomainData("DataSource=localhost")]
        [InlineAutoDomainData(42)]
        public void ShouldReturnNoSpecimenForNonOptionsRequestt(
            object request,
            Mock<ISpecimenContext> contextMock,
            [Frozen] Mock<IRequestSpecification> optionsSpecification)
        {
            var optionsType = typeof(DbContextOptions<TestDbContext>);
            var options = new DbContextOptions<TestDbContext>();
            var builder = new DelegatingDbContextOptionsBuilder(optionsSpecification.Object)
            {
                BuildOptions = _ => options
            };
            optionsSpecification
                .Setup(x => x.IsSatisfiedBy(It.Is<Type>(x => x == optionsType)))
                .Returns(true);

            var actual = builder.Create(request, contextMock.Object);

            actual.Should().BeOfType<NoSpecimen>();
        }
    }
}
