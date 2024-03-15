using PriceTracker.Data.PriceHistory.Commands;
using System;

namespace PriceTracker.Testing.PriceHistory.Commands;

public class CreatePriceHistoryTests : IClassFixture<BaseTestFixture>, IAsyncLifetime
{
    private readonly BaseTestFixture _fixture;

    private Data.Entity.UnitOfMeasure _ouncesUoM = new Data.Entity.UnitOfMeasure() { Name = "ounce", Abbreviation = "oz" };

    private Data.Entity.Product _product = new Data.Entity.Product() { Name = "Banana" };

    private Data.Entity.Store _store = new Data.Entity.Store() { Name = "Amazon" };

    public CreatePriceHistoryTests(BaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        await _fixture.AddAsync(_ouncesUoM);
        await _fixture.AddAsync(_product);
        await _fixture.AddAsync(_store);
    }

    [Fact]
    public async Task Create_With_Valid_Values_Is_Possible()
    {
        // Arrange        
        var createCommand = new CreatePriceHistory() { ProductId = _product.Id, UnitOfMeasureId = _ouncesUoM.Id, StoreId = _store.Id, Date = DateTime.Now, Quantity = 16, Price = 0.99m };

        // Act
        var result = await _fixture.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.ProductId.Should().Be(createCommand.ProductId);
        result.Value.UnitOfMeasureId.Should().Be(createCommand.UnitOfMeasureId);
        result.Value.StoreId.Should().Be(createCommand.StoreId);
        result.Value.Date.Should().Be(createCommand.Date);
        result.Value.Quantity.Should().Be(createCommand.Quantity);
        result.Value.Price.Should().Be(createCommand.Price);
    }

    [Fact]
    public async Task Create_With_Null_StoreId_Is_Possible()
    {
        // Arrange        
        var createCommand = new CreatePriceHistory() { ProductId = _product.Id, UnitOfMeasureId = _ouncesUoM.Id, StoreId = null, Date = DateTime.Now, Quantity = 16, Price = 0.99m };

        // Act
        var result = await _fixture.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.ProductId.Should().Be(createCommand.ProductId);
        result.Value.UnitOfMeasureId.Should().Be(createCommand.UnitOfMeasureId);
        result.Value.StoreId.Should().Be(createCommand.StoreId);
        result.Value.Date.Should().Be(createCommand.Date);
        result.Value.Quantity.Should().Be(createCommand.Quantity);
        result.Value.Price.Should().Be(createCommand.Price);
    }

    [Fact]
    public async Task Create_With_Negative_Quantity_Is_Not_Possible()
    {
        // Arrange
        var createCommand = new CreatePriceHistory() { ProductId = _product.Id, UnitOfMeasureId = _ouncesUoM.Id, StoreId = _store.Id, Date = DateTime.Now, Quantity = -16, Price = 0.99m };

        // Act
        var result = await _fixture.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_With_Negative_Price_Is_Not_Possible()
    {
        // Arrange
        var createCommand = new CreatePriceHistory() { ProductId = _product.Id, UnitOfMeasureId = _ouncesUoM.Id, StoreId = _store.Id, Date = DateTime.Now, Quantity = 16, Price = -0.99m };

        // Act
        var result = await _fixture.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_With_Zero_Quantity_Is_Not_Possible()
    {
        // Arrange
        var createCommand = new CreatePriceHistory() { ProductId = _product.Id, UnitOfMeasureId = _ouncesUoM.Id, StoreId = _store.Id, Date = DateTime.Now, Quantity = 0, Price = 0.99m };

        // Act
        var result = await _fixture.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_With_Zero_Price_Is_Not_Possible()
    {
        // Arrange
        var createCommand = new CreatePriceHistory() { ProductId = _product.Id, UnitOfMeasureId = _ouncesUoM.Id, StoreId = _store.Id, Date = DateTime.Now, Quantity = 16, Price = 0 };

        // Act
        var result = await _fixture.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_With_Invalid_ProductId_Is_Not_Possible()
    {
        // Arrange
        var createCommand = new CreatePriceHistory() { ProductId = 0, UnitOfMeasureId = _ouncesUoM.Id, StoreId = _store.Id, Date = DateTime.Now, Quantity = 16 };

        // Act
        var result = await _fixture.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_With_Invalid_UnitOfMeasureId_Is_Not_Possible()
    {
        // Arrange
        var createCommand = new CreatePriceHistory() { ProductId = _product.Id, UnitOfMeasureId = 0, StoreId = _store.Id, Date = DateTime.Now, Quantity = 16 };

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
