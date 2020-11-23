using System;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace AutoFixture.Community.AutoEF
{
    public class DbSetSpecification : IRequestSpecification
    {
        public bool IsSatisfiedBy(object request)
            => request is Type type
            && type.IsGenericType
            && type.GenericTypeArguments.Length == 1
            && type.GetGenericTypeDefinition() == typeof(DbSet<>);
    }
}
