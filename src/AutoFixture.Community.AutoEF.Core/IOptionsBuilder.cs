using System;

namespace AutoFixture.Community.AutoEF
{
    public interface IOptionsBuilder
    {
        object Build(Type type);
    }
}
