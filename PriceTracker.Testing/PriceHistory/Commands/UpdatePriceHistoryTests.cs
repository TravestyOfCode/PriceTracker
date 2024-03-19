using PriceTracker.Data.PriceHistory.Commands;
using System;
using System.Collections.Generic;

namespace PriceTracker.Testing.PriceHistory.Commands;

public class UpdatePriceHistoryTests : IAsyncLifetime, IClassFixture<BaseTestFixture>
{
    private readonly BaseTestFixture _fixture;

    private List<Data.Entity.Product> products;

    private List<Data.Entity.UnitOfMeasure> uoms;

    private List<Data.Entity.PriceHistory> priceHistories;

    public UpdatePriceHistoryTests(BaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        // Seed data
        uoms = new List<Data.Entity.UnitOfMeasure>()
        {
            new Data.Entity.UnitOfMeasure(){ Name = "ounce", Abbreviation = "oz" },
            new Data.Entity.UnitOfMeasure(){ Name = "gram", Abbreviation = "g" }
        };
        await _fixture.AddRangeAsync(uoms);

        products = new List<Data.Entity.Product>()
        {
            new Data.Entity.Product() { Name = "Banana", DefaultUnitOfMeasureId = uoms[0].Id },
            new Data.Entity.Product() { Name = "Orange", DefaultUnitOfMeasureId = uoms[1].Id },
            new Data.Entity.Product() { Name = "Grapes" }
        };
        await _fixture.AddRangeAsync(products);

        priceHistories = new List<Data.Entity.PriceHistory>()
        {
            new Data.Entity.PriceHistory() {ProductId = products[0].Id, UnitOfMeasureId = uoms[0].Id, Quantity = 16, Price = 0.99m, Date = DateTime.Now },
            new Data.Entity.PriceHistory() {ProductId = products[1].Id, UnitOfMeasureId = uoms[0].Id, Quantity = 17, Price = 1.99m, Date = DateTime.Now },
            new Data.Entity.PriceHistory() {ProductId = products[2].Id, UnitOfMeasureId = uoms[0].Id, Quantity = 18, Price = 2.99m, Date = DateTime.Now },
        };
        await _fixture.AddRangeAsync(priceHistories);
    }

    [Fact]
    public async Task Update_With_Valid_Values_Is_Possible()
    {
        // Arrange
        var command = new UpdatePriceHistory() { Id = priceHistories[0].Id, ProductId = products[1].Id, UnitOfMeasureId = uoms[1].Id, Quantity = 10, Price = .59m, Date = DateTime.Now.AddDays(-15) };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.ProductId.Should().Be(command.ProductId);
        result.Value.UnitOfMeasureId.Should().Be(command.UnitOfMeasureId);
        result.Value.Quantity.Should().Be(command.Quantity);
        result.Value.Price.Should().Be(command.Price);
        result.Value.Date.Should().Be(command.Date);
    }

    [Fact]
    public async Task Update_With_Negative_Quantity_Is_Not_Possible()
    {
        // Arrange
        var createCommand = new UpdatePriceHistory() { ProductId = products[0].Id, UnitOfMeasureId = uoms[0].Id, Date = DateTime.Now, Quantity = -16, Price = 0.99m };

        // Act
        var result = await _fixture.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_With_Negative_Price_Is_Not_Possible()
    {
        // Arrange
        var createCommand = new UpdatePriceHistory() { ProductId = products[0].Id, UnitOfMeasureId = uoms[0].Id, Date = DateTime.Now, Quantity = 16, Price = -0.99m };

        // Act
        var result = await _fixture.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_With_Zero_Quantity_Is_Not_Possible()
    {
        // Arrange
        var createCommand = new UpdatePriceHistory() { ProductId = products[0].Id, UnitOfMeasureId = uoms[0].Id, Date = DateTime.Now, Quantity = 0, Price = 0.99m };

        // Act
        var result = await _fixture.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_With_Zero_Price_Is_Not_Possible()
    {
        // Arrange
        var createCommand = new UpdatePriceHistory() { ProductId = products[0].Id, UnitOfMeasureId = uoms[0].Id, Date = DateTime.Now, Quantity = 16, Price = 0 };

        // Act
        var result = await _fixture.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_With_Invalid_ProductId_Is_Not_Possible()
    {
        // Arrange
        var createCommand = new UpdatePriceHistory() { ProductId = 0, UnitOfMeasureId = uoms[0].Id, Date = DateTime.Now, Quantity = 16 };

        // Act
        var result = await _fixture.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_With_Invalid_UnitOfMeasureId_Is_Not_Possible()
    {
        // Arrange
        var createCommand = new UpdatePriceHistory() { ProductId = products[0].Id, UnitOfMeasureId = 0, Date = DateTime.Now, Quantity = 16 };

        // Act
        var result = await _fixture.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    public async Task DisposeAsync()
    {
        await _fixture.ResetDatabase();
    }

}
