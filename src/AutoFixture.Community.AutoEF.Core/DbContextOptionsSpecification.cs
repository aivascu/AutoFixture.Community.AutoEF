using System;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace AutoFixture.Community.AutoEF
{
    public class DbContextOptionsSpecification : IRequestSpecification
    {
        public bool IsSatisfiedBy(object request)
        {
            return request is Type type
                   && !type.IsAbstract
                   && type.IsGenericType
                   && type.GenericTypeArguments.Length == 1
                   && typeof(DbContextOptions<>) == type.GetGenericTypeDefinition();
        }
    }
}
