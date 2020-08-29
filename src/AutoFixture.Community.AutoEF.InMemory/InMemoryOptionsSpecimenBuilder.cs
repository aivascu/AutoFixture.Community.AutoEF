using System;
using AutoFixture.Kernel;

namespace AutoFixture.Community.AutoEF.InMemory
{
    public class InMemoryOptionsSpecimenBuilder : ISpecimenBuilder
    {
        public InMemoryOptionsSpecimenBuilder(IRequestSpecification optionsSpecification)
        {
            this.OptionsSpecification = optionsSpecification
                                        ?? throw new ArgumentNullException(nameof(optionsSpecification));
        }

        public InMemoryOptionsSpecimenBuilder()
            : this(new IsOptionsBuilder())
        {
        }

        public IRequestSpecification OptionsSpecification { get; }

        public object Create(object request, ISpecimenContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!this.OptionsSpecification.IsSatisfiedBy(request))
            {
                return new NoSpecimen();
            }

            return new InMemoryOptionsBuilder();
        }

        private class IsOptionsBuilder : IRequestSpecification
        {
            public bool IsSatisfiedBy(object request)
            {
                return request is Type type
                       && type.IsInterface
                       && type == typeof(IOptionsBuilder);
            }
        }
    }
}
