using AutoFixture.Xunit2;

namespace AutoFixture.Community.AutoEF.Sqlite.Tests
{
    public class AutoSqliteDomainDataAttribute : AutoDataAttribute
    {
        public AutoSqliteDomainDataAttribute()
            : base(() => new Fixture()
                .Customize(new SqliteDomainCustomization()))
        {
        }
    }
}
