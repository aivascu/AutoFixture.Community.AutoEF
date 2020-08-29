using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.Infrastructure.Internal;
using TestClasses;
using Xunit;

namespace AutoFixture.Community.AutoEF.InMemory.Tests
{
    public class InMemoryOptionsBuilderTests
    {
        [Theory]
        [AutoData]
        public void Build_ShouldBuildDbContextOptionsInstance(InMemoryOptionsBuilder builder)
        {
            var options = builder.Build(typeof(TestDbContext));

            options.Should().BeOfType<DbContextOptions<TestDbContext>>();
        }

        [Theory]
        [AutoData]
        public void Build_ShouldThrowArgumentNullException_WhenTypeIsNull(InMemoryOptionsBuilder builder)
        {
            Action action = () => builder.Build(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [AutoData]
        public void Build_ShouldThrowArgumentNullException_WhenTypeIsNotDbContext(InMemoryOptionsBuilder builder)
        {
            Action action = () => builder.Build(typeof(string));

            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [AutoData]
        public void Build_ShouldThrowArgumentNullException_WhenTypeIsAbstract(InMemoryOptionsBuilder builder)
        {
            Action action = () => builder.Build(typeof(AbstractDbContext));

            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [AutoData]
        public void Constructors_ShouldReceiveInitializedParameters(Fixture fixture)
        {
            var assertion = new GuardClauseAssertion(fixture);
            var members = typeof(InMemoryOptionsBuilder).GetConstructors();

            assertion.Verify(members);
        }

        [Theory]
        [AutoData]
        public void GenericBuild_ShouldCreateDbContextOptions_WithInMemoryExtension(InMemoryOptionsBuilder builder)
        {
            var actual = builder.Build<TestDbContext>();

            actual.Extensions.Should().Contain(x => x.GetType() == typeof(InMemoryOptionsExtension));
        }

        [Theory]
        [AutoData]
        [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.",
            Justification = "Asserts the internal state of EF storage")]
        public void GenericBuild_ShouldCreateDbContextOptions_WithInMemoryExtension_WithName(string expected)
        {
            var extension = new InMemoryOptionsBuilder(expected)
                .Build<TestDbContext>()
                .Extensions
                .Single(x => x.GetType() == typeof(InMemoryOptionsExtension))
                .As<InMemoryOptionsExtension>();

            extension.StoreName.Should().Be(expected);
        }

        [Theory]
        [AutoData]
        public void Build_ShouldCreateDbContextOptions_WithInMemoryExtension(InMemoryOptionsBuilder builder)
        {
            var actual = builder.Build(typeof(TestDbContext)).As<DbContextOptions<TestDbContext>>();

            actual.Extensions.Should().Contain(x => x.GetType() == typeof(InMemoryOptionsExtension));
        }

        [Theory]
        [AutoData]
        [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.",
            Justification = "Asserts the internal state of EF storage")]
        public void Build_ShouldCreateDbContextOptions_WithInMemoryExtension_WithName(string expected)
        {
            var extension = new InMemoryOptionsBuilder(expected)
                .Build(typeof(TestDbContext))
                .As<DbContextOptions<TestDbContext>>()
                .Extensions
                .Single(x => x.GetType() == typeof(InMemoryOptionsExtension))
                .As<InMemoryOptionsExtension>();

            extension.StoreName.Should().Be(expected);
        }

        private abstract class AbstractDbContext : DbContext
        {
        }
    }
}
