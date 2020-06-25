using AutoFixture.AutoMoq;

namespace AutoFixture.Community.AutoEF.InMemory.Tests
{
    public class InMemoryDomainCustomization : CompositeCustomization
    {
        public InMemoryDomainCustomization()
            : base(
                new IgnoredVirtualMembersCustomization(),
                new InMemoryContextCustomization(),
                new AutoMoqCustomization())
        {
        }
    }
}
