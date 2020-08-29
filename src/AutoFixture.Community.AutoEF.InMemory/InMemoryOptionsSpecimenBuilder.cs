using System;
using AutoFixture.Kernel;

namespace AutoFixture.Community.AutoEF.InMemory
{
    public class InMemoryOptionsSpecimenBuilder : ISpecimenBuilder
    {
        public InMemoryOptionsSpecimenBuilder(
            IRequestSpecification optionsSpecification)
        {
            this.OptionsBuilderSpecification = optionsSpecification
                ?? throw new ArgumentNullException(nameof(optionsSpecification));
        }

        public InMemoryOptionsSpecimenBuilder()
            : this(new OptionsBuilderSpecification())
        {
        }

        public IRequestSpecification OptionsBuilderSpecification { get; }

        public object Create(object request, ISpecimenContext context)
        {
            return this.OptionsBuilderSpecification.IsSatisfiedBy(request)
                ? new InMemoryOptionsBuilder()
                : (object)new NoSpecimen();
        }
    }
}
