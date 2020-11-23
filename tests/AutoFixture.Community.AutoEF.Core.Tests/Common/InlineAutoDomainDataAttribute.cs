using AutoFixture.Xunit2;

namespace AutoFixture.Community.AutoEF.Core.Tests
{
    public class InlineAutoDomainDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoDomainDataAttribute(params object[] attributes)
            : base(new AutoDomainDataAttribute(), attributes)
        {
        }
    }
}
