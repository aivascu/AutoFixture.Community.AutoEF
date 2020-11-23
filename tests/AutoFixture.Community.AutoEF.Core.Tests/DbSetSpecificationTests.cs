using System;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TestClasses.Entities;
using Xunit;

namespace AutoFixture.Community.AutoEF.Core.Tests
{
    public class DbSetSpecificationTests
    {
        [Theory]
        [InlineAutoData(typeof(DbSet<Customer>))]
        [InlineAutoData(typeof(DbSet<Order>))]
        public void ShouldReturnTrueForDbSetTypes(
            Type type, DbSetSpecification specification)
        {
            specification.IsSatisfiedBy(type)
                .Should().BeTrue();
        }

        [Theory]
        [InlineAutoData(typeof(DbSet<>))]
        [InlineAutoData(typeof(Order))]
        [InlineAutoData(typeof(string))]
        [InlineAutoData(typeof(int))]
        public void ShouldReturnFalseForNonDbSetTypes(
            Type type, DbSetSpecification specification)
        {
            specification.IsSatisfiedBy(type)
                .Should().BeFalse();
        }
    }
}
