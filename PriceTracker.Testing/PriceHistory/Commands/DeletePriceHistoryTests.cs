using PriceTracker.Data.PriceHistory.Commands;
using System;

namespace PriceTracker.Testing.PriceHistory.Commands;

public class DeletePriceHistoryTests : IAsyncLifetime, IClassFixture<BaseTestFixture>
{
    private readonly BaseTestFixture _fixture;

    private Data.Entity.UnitOfMeasure _ouncesUoM = new Data.Entity.UnitOfMeasure() { Name = "ounce", Abbreviation = "oz" };

    private Data.Entity.Product _product = new Data.Entity.Product() { Name = "Banana" };

    private Data.Entity.Store _store = new Data.Entity.Store() { Name = "Amazon" };

    public DeletePriceHistoryTests(BaseTestFixture fixture)
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
    public async Task Delete_With_Valid_Id_Is_Possible()
    {
        // Assert
        var entity = new Data.Entity.PriceHistory() { ProductId = _product.Id, UnitOfMeasureId = _ouncesUoM.Id, Date = DateTime.Now, Price = 1.99m, Quantity = 16 };
        await _fixture.AddAsync(entity);
        var command = new DeletePriceHistory() { Id = entity.Id };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_With_Invalid_Id_Is_Not_Possible()
    {
        // Assert
        var command = new DeletePriceHistory() { Id = 0 };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeFalse();
    }

    public async Task DisposeAsync()
    {
        await _fixture.ResetDatabase();
    }
}
