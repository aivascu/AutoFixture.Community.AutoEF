using AutoFixture.Xunit2;

namespace AutoFixture.Community.AutoEF.InMemory.Tests
{
    public class AutoInMemoryDomainDataAttribute : AutoDataAttribute
    {
        public AutoInMemoryDomainDataAttribute()
            : base(() => new Fixture()
                .Customize(new InMemoryDomainCustomization()))
        {
        }
    }
}
