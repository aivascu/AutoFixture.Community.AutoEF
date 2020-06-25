using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Community.AutoEF;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Core
{
    public class DbContextCustomizationTests
    {
        [Theory]
        [AutoData]
        public void Customize_ShouldAddOptionsBuilderToFixture(
            Fixture fixture,
            DbContextCustomization customization)
        {
            fixture.Customize(customization);

            fixture.Customizations.Should()
                .ContainSingle(x => x.GetType() == typeof(DbContextOptionsSpecimenBuilder));
        }

        [Theory]
        [AutoData]
        public void Customize_ForNullFixture_ShouldThrow(
            DbContextCustomization customization)
        {
            Action act = () => customization.Customize(default);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
