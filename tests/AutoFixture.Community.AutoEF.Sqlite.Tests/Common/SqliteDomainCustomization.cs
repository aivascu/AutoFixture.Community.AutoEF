using AutoFixture.AutoMoq;

namespace AutoFixture.Community.AutoEF.Sqlite.Tests
{
    public class SqliteDomainCustomization : CompositeCustomization
    {
        public SqliteDomainCustomization()
            : base(
                new SqliteContextCustomization(),
                new AutoMoqCustomization())
        {
        }
    }
}
