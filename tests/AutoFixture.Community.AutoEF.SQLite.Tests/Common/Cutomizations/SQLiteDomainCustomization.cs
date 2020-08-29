using AutoFixture.AutoMoq;

namespace AutoFixture.Community.AutoEF.Sqlite.Tests
{
    public class SQLiteDomainCustomization : CompositeCustomization
    {
        public SQLiteDomainCustomization()
            : base(
                new IgnoredVirtualMembersCustomization(),
                new SqliteContextCustomization(),
                new AutoMoqCustomization())
        {
        }
    }
}
