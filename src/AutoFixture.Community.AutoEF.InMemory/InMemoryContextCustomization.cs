using System;

namespace AutoFixture.Community.AutoEF.InMemory
{
    public class InMemoryContextCustomization : DbContextCustomization
    {
        public override void Customize(IFixture fixture)
        {
            if (fixture is null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            base.Customize(fixture);

            fixture.Customizations.Add(new InMemoryOptionsSpecimenBuilder());
        }
    }
}
