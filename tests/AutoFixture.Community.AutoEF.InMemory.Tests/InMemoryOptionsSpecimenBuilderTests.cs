using System;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using FluentAssertions;
using Moq;
using Xunit;

namespace AutoFixture.Community.AutoEF.InMemory.Tests
{
    public class InMemoryOptionsSpecimenBuilderTests
    {
        [Theory(DisplayName =
            "Create should create InMemoryOptionsBuilder instance when request type is IOptionsBuilder")]
        [AutoDomainData]
        public void Create_ShouldCreateInMemoryOptionsBuilder_WhenRequestTypeIsOptionsBuilderInterface(
            Mock<ISpecimenContext> context,
            InMemoryOptionsSpecimenBuilder builder)
        {
            var builderObj = builder.Create(typeof(IOptionsBuilder), context.Object);

            builderObj.Should().BeOfType<InMemoryOptionsBuilder>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldThrow_WhenSpecimenContextIsNull(InMemoryOptionsSpecimenBuilder builder)
        {
            Action act = () => builder.Create(typeof(IOptionsBuilder), null);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [AutoDomainData]
        public void Create_ShouldReturnNoSpecimen_WhenRequestTypeNotOptionsBuilderInterface(
            Mock<ISpecimenContext> context,
            InMemoryOptionsSpecimenBuilder builder)
        {
            var obj = builder.Create(typeof(string), context.Object);

            obj.Should().BeOfType<NoSpecimen>();
        }

        [Theory]
        [AutoDomainData]
        public void Constructors_ShouldReceiveInitializedParameters(GuardClauseAssertion assertion)
        {
            var members = typeof(InMemoryOptionsSpecimenBuilder).GetConstructors();

            assertion.Verify(members);
        }
    }
}
