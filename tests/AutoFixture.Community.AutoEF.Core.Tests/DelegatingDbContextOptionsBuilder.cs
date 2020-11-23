using System;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace AutoFixture.Community.AutoEF.Core.Tests
{
    public class DelegatingDbContextOptionsBuilder : DbContextOptionsBuilder
    {
        public DelegatingDbContextOptionsBuilder(IRequestSpecification optionsSpecification)
            : base(optionsSpecification)
        {
        }

        public Func<ISpecimenContext, DbContextOptions> BuildOptions { get; set; }

        protected override DbContextOptions<TContext> Build<TContext>(ISpecimenContext context)
        {
            return this.BuildOptions(context) as DbContextOptions<TContext>;
        }
    }
}
