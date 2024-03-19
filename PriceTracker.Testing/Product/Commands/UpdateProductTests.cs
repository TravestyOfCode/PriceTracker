using PriceTracker.Data.Product.Commands;
using System.Collections.Generic;

namespace PriceTracker.Testing.Product.Commands;

public class UpdateProductTests : IAsyncLifetime, IClassFixture<BaseTestFixture>
{
    private readonly BaseTestFixture _fixture;

    private List<Data.Entity.Product> products;

    private List<Data.Entity.UnitOfMeasure> uoms;

    public UpdateProductTests(BaseTestFixture fixture)
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
    }

    [Fact]
    public async Task Update_With_Valid_Values_Is_Possible()
    {
        // Arrange
        var command = new UpdateProduct() { Id = products[0].Id, Name = "Bananas", DefaultUnitOfMeasureId = uoms[1].Id };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be(command.Name);
        result.Value.DefaultUnitOfMeasureId.Should().Be(command.DefaultUnitOfMeasureId);
    }

    [Fact]
    public async Task Update_With_Duplicate_Name_Is_Not_Possible()
    {
        // Arrange
        var command = new UpdateProduct() { Id = products[1].Id, Name = "Grapes", DefaultUnitOfMeasureId = uoms[0].Id };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_With_Empty_Name_Is_Not_Possible()
    {
        // Arrange
        var command = new UpdateProduct() { Id = products[1].Id, Name = "", DefaultUnitOfMeasureId = uoms[1].Id };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_With_Empty_DefaultUnitOfMeasure_Is_Possible()
    {
        // Arrange
        var command = new UpdateProduct() { Id = products[1].Id, Name = "Orange", DefaultUnitOfMeasureId = null };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be(command.Name);
        result.Value.DefaultUnitOfMeasureId.Should().BeNull();
    }

    [Fact]
    public async Task Update_With_Invalid_Id_Is_Not_Possible()
    {
        // Arrange
        var command = new UpdateProduct() { Id = 0, Name = "Strawberry", DefaultUnitOfMeasureId = uoms[0].Id };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_With_Invalid_DefaultUnitOfMeasureId_Is_Not_Possible()
    {
        // Arrange
        var command = new UpdateProduct() { Id = products[0].Id, Name = "Strawberry", DefaultUnitOfMeasureId = -1 };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    public async Task DisposeAsync()
    {
        await _fixture.ResetDatabase();
    }

}
