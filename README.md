# AutoFixture.Community.AutoEF

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/aivascu/AutoFixture.Community.AutoEF/continuous?logo=github&style=flat-square)
[![Total alerts](https://img.shields.io/lgtm/alerts/g/aivascu/AutoFixture.Community.AutoEF.svg?logo=lgtm&logoWidth=18&style=flat-square)](https://lgtm.com/projects/g/aivascu/AutoFixture.Community.AutoEF/alerts/)
[![GitHub](https://img.shields.io/github/license/aivascu/AutoFixture.Community.AutoEF?logo=MIT&style=flat-square)](https://licenses.nuget.org/MIT)

**AutoFixture.Community.AutoEF** is the logical product of [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) in-memory providers and [AutoFixture](https://github.com/AutoFixture/AutoFixture).

Using **AutoFixture.Community.AutoEF** you can greatly reduce the boilerplate work necessary to unit test code that uses **Entity Framework Core** database contexts (see [examples](#examples)). You'll appreciate this library if you are already using **AutoFixture** as your auto-mocking container.

**AutoFixture.Community.AutoEF** extens **AutoFixture** with the ability to create fully functional `DbContext` instances, with very little setup code.

Unlike other libraries for faking EF contexts, **AutoFixture.Community.AutoEF** does not use mocking frameworks or dynamic proxies in order to create `DbContext` instances, instead it uses the Microsoft's own in-memory [providers](https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/) for EF Core. This allows to make less assumptions (read as: mock setups) in your tests about how the `DbContext` will behave in the real environment.

|Package|Version|
|-------|-------|
|AutoFixture.Community.AutoEF.Core|[![Nuget](https://img.shields.io/nuget/v/AutoFixture.Community.AutoEF.Core?logo=nuget&style=flat-square)](https://www.nuget.org/packages/AutoFixture.Community.AutoEF.Core/)|
|AutoFixture.Community.AutoEF.Sqlite|[![Nuget](https://img.shields.io/nuget/v/AutoFixture.Community.AutoEF.Sqlite?logo=nuget&style=flat-square)](https://www.nuget.org/packages/AutoFixture.Community.AutoEF.Sqlite/)|
|AutoFixture.Community.AutoEF.InMemory|[![Nuget](https://img.shields.io/nuget/v/AutoFixture.Community.AutoEF.InMemory?logo=nuget&style=flat-square)](https://www.nuget.org/packages/AutoFixture.Community.AutoEF.InMemory/)|
## Features

Each **AutoFixture.Community.AutoEF** package offers a customization to aid your unit testing workflow:

- `InMemoryContextCustomization` - customizes fixtures to use the In-Memory database provider when creating *DbContext* instances
- `SqliteContextCustomization` - customizes fixtures to use the SQLite database provider when creating *DbContext* instances.
By default the customization will create contexts for an in-memory *connection string* (i.e. `DataSource=:memory:`). This can be changed by providing the fixture a predefined `SqliteConnection` instance.

## Examples

The examples below demonstrate, the possible ways of using the library in [xUnit](https://github.com/xunit/xunit) test projects, both with `[Fact]` and `[Theory]` tests.

The library is not limited to `xUnit` and can be used with other testing frameworks like `NUnit` and `MSTest`, since it only provides a few `Customization` implementations.

### Using In-Memory database provider

```csharp
[Fact]
public void SaveChanges_ShouldCreateCustomerRecord()
{
    var fixture = new Fixture()
        .Customize(new InMemoryContextCustomization());
    var context = fixture.Create<TestDbContext>();
    context.Database.EnsureCreated();

    context.Customers.Add(new Customer("John Doe"));
    context.SaveChanges();

    context.Customers.Should()
        .Contain(x => x.Name == "John Doe");
}
```

The next example uses a custom `AutoData` attribute `AutoDomainDataWithInMemoryContext` that customizes the fixture with the same customization as in the example above. This helps abstract away even more setup code. The attribute implementation can be found the sources of the test projects.

```csharp
[Theory]
[AutoDomainDataWithInMemoryContext]
public async Task SaveChangesAsync_ShouldCreateCustomerRecord(
    TestDbContext context)
{
    await context.Database.EnsureCreatedAsync();

    context.Customers.Add(new Customer("Jane Smith"));
    await context.SaveChangesAsync();

    context.Customers.Should()
        .Contain(x => x.Name == "Jane Smith");
}
```

### Using SQLite database provider

When using the SQLite database provider be sure to also *freeze* / *inject* the `SqliteConnection` instance, in order to be able to control its lifetime.
Otherwise the connection might close, which might in its turn fail your tests.

```csharp
[Theory]
[AutoDomainDataWithSqliteContext]
public void Customize_ShouldProvideSqliteContext(
    [Frozen] SqliteConnection connection,
    TestDbContext context, Item item,
    Customer customer)
{
    connection.Open();
    context.Database.EnsureCreated();
    context.Items.Add(item);

    context.Customers.Add(customer);
    context.SaveChanges();

    customer.Order(item, 5);
    context.SaveChanges();

    context.Orders.Should()
        .Contain(x => x.CustomerId == customer.Id 
            && x.ItemId == item.Id);
}
```

## License

Copyright &copy; 2019 [Andrei Ivascu](https://github.com/aivascu).<br/>
This project is [MIT](https://github.com/aivascu/AutoFixture.Community.AutoEF/blob/master/LICENSE) licensed.