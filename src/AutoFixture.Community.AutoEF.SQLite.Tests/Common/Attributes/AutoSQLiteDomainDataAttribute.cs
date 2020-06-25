using AutoFixture.Xunit2;

namespace AutoFixture.Community.AutoEF.SQLite.Tests
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
