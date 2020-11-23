using System;
using System.Threading.Tasks;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using FluentAssertions;
using TestClasses;
using TestClasses.Entities;
using Xunit;

namespace AutoFixture.Community.AutoEF.InMemory.Tests
{
    public class InMemoryCustomizationTests
    {
        [Fact]
        public void SaveChanges_ShouldCreateCustomerRecord()
        {
            var context = new Fixture()
                .Customize(
                    new CompositeCustomization(
                        new InMemoryContextCustomization(),
                        new ConstructorCustomization(
                            typeof(TestDbContext),
                            new GreedyConstructorQuery())))
                .Create<TestDbContext>();

            context.Database.EnsureCreated();

            context.Customers.Add(new Customer("John Doe"));
            context.SaveChanges();

            context.Customers.Should().Contain(x => x.Name == "John Doe");
        }

        [Theory]
        [AutoInMemoryDomainData]
        public async Task SaveChangesAsync_ShouldCreateCustomerRecord(
            [Greedy] TestDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            context.Customers.Add(new Customer("Jane Smith"));
            await context.SaveChangesAsync();

            context.Customers.Should().Contain(x => x.Name == "Jane Smith");
        }

        [Fact]
        public void Customize_ShouldAddOptionsBuilderToFixture()
        {
            var fixture = new Fixture().Customize(new InMemoryContextCustomization());

            fixture.Customizations.Should()
                .ContainSingle(x => x.GetType() == typeof(InMemoryOptionsBuilder));
        }

        [Fact]
        public void Customize_ForNullFixture_ShouldThrow()
        {
            Action act = () => new InMemoryContextCustomization().Customize(default);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [AutoInMemoryDomainData]
        public void Customize_ForCustomDbContext_ShouldReturnContextInstance(
            [Greedy] TestCustomDbContext context)
        {
            context.Should().NotBeNull()
                .And.BeOfType<TestCustomDbContext>();
        }

        [Theory]
        [AutoInMemoryDomainData]
        public void Customize_ForCustomDbContext_ProvideValueForOtherParameters(
            [Greedy] TestCustomDbContext context)
        {
            context.ConfigurationOptions.Should().NotBeNull();
        }
    }
}
