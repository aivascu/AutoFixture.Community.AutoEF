using System;
using AutoFixture.Kernel;

namespace AutoFixture.Community.AutoEF
{
    public class OptionsBuilderSpecification : IRequestSpecification
    {
        public bool IsSatisfiedBy(object request)
        {
            return request is Type type
                   && type.IsInterface
                   && type == typeof(IOptionsBuilder);
        }
    }
}
