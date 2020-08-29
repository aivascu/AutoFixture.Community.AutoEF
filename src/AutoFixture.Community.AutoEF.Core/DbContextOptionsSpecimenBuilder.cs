using System;
using System.Linq;
using AutoFixture.Kernel;

namespace AutoFixture.Community.AutoEF
{
    public class DbContextOptionsSpecimenBuilder : ISpecimenBuilder
    {
        public DbContextOptionsSpecimenBuilder()
            : this(new DbContextOptionsSpecification())
        {
        }

        public DbContextOptionsSpecimenBuilder(IRequestSpecification optionsSpecification)
        {
            this.DbContextOptionsSpecification = optionsSpecification
                ?? throw new ArgumentNullException(nameof(optionsSpecification));
        }

        public IRequestSpecification DbContextOptionsSpecification { get; }

        public object Create(object request, ISpecimenContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!this.DbContextOptionsSpecification.IsSatisfiedBy(request))
            {
                return new NoSpecimen();
            }

            var optionsBuilderObj = context.Resolve(typeof(IOptionsBuilder));

            return optionsBuilderObj is IOptionsBuilder optionsBuilder
                ? optionsBuilder.Build(((Type)request).GetGenericArguments().Single())
                : new NoSpecimen();
        }
    }
}
