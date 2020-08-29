using System;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using TestClasses;
using TestClasses.Entities;
using Xunit;

namespace AutoFixture.Community.AutoEF.SQLite.Tests
{
    public class SQLiteCustomizationTests
    {
        [Theory]
        [AutoSQLiteDomainData]
        public void Customize_ShouldProvideSqliteContext(
            [Frozen] SqliteConnection connection,
            [Greedy] TestDbContext context,
            Item item, Customer customer)
        {
            using (connection)
            using (context)
            {
                connection.Open();
                context.Database.EnsureCreated();
                context.Items.Add(item);

                context.Customers.Add(customer);
                context.SaveChanges();

                customer.Order(item, 5);
                context.SaveChanges();

                context.Orders.Should()
                    .Contain(x => x.CustomerId == customer.Id && x.ItemId == item.Id);
            }
        }

        [Theory]
        [AutoData]
        public void Customize_ShouldAddOptionsBuilderToFixture(
            Fixture fixture,
            SqliteContextCustomization customization)
        {
            fixture.Customize(customization);

            fixture.Customizations.Should()
                .ContainSingle(x => x.GetType() == typeof(SqliteOptionsSpecimenBuilder));
        }

        [Theory]
        [AutoData]
        public void Customize_ShouldAddConnectionBuilderToFixture(
            Fixture fixture,
            SqliteContextCustomization customization)
        {
            fixture.Customize(customization);

            fixture.Customizations.Should()
                .ContainSingle(x => x.GetType() == typeof(SqliteConnectionSpecimenBuilder));
        }

        [Theory]
        [AutoData]
        public void Customize_ForNullFixture_ShouldThrow(
            SqliteContextCustomization customization)
        {
            Action act = () => customization.Customize(default);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [AutoSQLiteDomainData]
        public void Customize_ForCustomDbContext_ShouldReturnContextInstance(
            [Greedy] TestCustomDbContext context)
        {
            context.Should().NotBeNull()
                .And.BeOfType<TestCustomDbContext>();
        }

        [Theory]
        [AutoSQLiteDomainData]
        public void Customize_ForCustomDbContext_ProvideValueForOtherParameters(
            [Greedy] TestCustomDbContext context)
        {
            context.ConfigurationOptions.Should().NotBeNull();
        }
    }
}
