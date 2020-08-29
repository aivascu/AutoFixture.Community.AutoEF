using AutoFixture.Xunit2;

namespace AutoFixture.Community.AutoEF.Sqlite.Tests
{
    public class AutoSQLiteDomainDataAttribute : AutoDataAttribute
    {
        public AutoSQLiteDomainDataAttribute()
            : base(() => new Fixture()
                .Customize(new SQLiteDomainCustomization()))
        {
        }
    }
}
