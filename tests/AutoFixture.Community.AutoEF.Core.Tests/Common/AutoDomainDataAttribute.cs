using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace AutoFixture.Community.AutoEF.Core.Tests
{
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute()
            : base(() => new Fixture()
                .Customize(new AutoMoqCustomization()))
        {
        }
    }
}
