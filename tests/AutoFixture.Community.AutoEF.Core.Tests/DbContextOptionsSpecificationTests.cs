using System;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TestClasses;
using TestClasses.Entities;
using Xunit;

namespace AutoFixture.Community.AutoEF.Core.Tests
{
    public class DbContextOptionsSpecificationTests
    {
        [Theory]
        [InlineAutoData(typeof(DbContextOptions<TestDbContext>))]
        [InlineAutoData(typeof(DbContextOptions<TestCustomDbContext>))]
        public void ShouldReturnTrueForDbContexttTypes(
            Type type, DbContextOptionsSpecification specification)
        {
            specification.IsSatisfiedBy(type)
                .Should().BeTrue();
        }

        [Theory]
        [InlineAutoData(typeof(DbContextOptions<>))]
        [InlineAutoData(typeof(Order))]
        [InlineAutoData(typeof(string))]
        [InlineAutoData(typeof(int))]
        public void ShouldReturnFalseForNonDbSetTypes(
            Type type, DbContextOptionsSpecification specification)
        {
            specification.IsSatisfiedBy(type)
                .Should().BeFalse();
        }
    }
}
