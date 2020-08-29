﻿using System;
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
        [Theory]
        [AutoData]
        public void SaveChanges_ShouldCreateCustomerRecord(
            InMemoryContextCustomization customization,
            Fixture fixture)
        {
            fixture.Customize(
                new CompositeCustomization(
                    customization,
                    new ConstructorCustomization(
                        typeof(TestDbContext),
                        new GreedyConstructorQuery())));

            using var context = fixture.Create<TestDbContext>();
            context.Database.EnsureCreated();

            context.Customers.Add(new Customer("John Doe"));
            context.SaveChanges();

            context.Customers.Should().Contain(x => x.Name == "John Doe");
        }

        [Theory]
        [AutoInMemoryDomainData]
        public async Task SaveChangesAsync_ShouldCreateCustomerRecord([Greedy] TestDbContext context)
        {
            await using (context)
            {
                await context.Database.EnsureCreatedAsync();

                context.Customers.Add(new Customer("Jane Smith"));
                await context.SaveChangesAsync();

                context.Customers.Should().Contain(x => x.Name == "Jane Smith");
            }
        }

        [Theory]
        [AutoData]
        public void Customize_ShouldAddOptionsBuilderToFixture(InMemoryContextCustomization customization,
            Fixture fixture)
        {
            fixture.Customize(customization);

            fixture.Customizations.Should()
                .ContainSingle(x => x.GetType() == typeof(InMemoryOptionsSpecimenBuilder));
        }

        [Theory]
        [AutoData]
        public void Customize_ForNullFixture_ShouldThrow(InMemoryContextCustomization customization)
        {
            Action act = () => customization.Customize(default);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [AutoInMemoryDomainData]
        public void Customize_ForCustomDbContext_ShouldReturnContextInstance([Greedy] TestCustomDbContext context)
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
