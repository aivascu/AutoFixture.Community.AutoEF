﻿namespace AutoFixture.Community.AutoEF.InMemory.Tests
{
    public class IgnoredVirtualMembersCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new IgnoredVirtualMembersSpecimenBuilder());
        }
    }
}