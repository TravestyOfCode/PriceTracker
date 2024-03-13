using PriceTracker.Data.Product.Commands;
using System.Collections.Generic;

namespace PriceTracker.Testing.Product.Commands;

public class DeleteProductTests : IAsyncLifetime, IClassFixture<BaseTestFixture>
{
    private readonly BaseTestFixture _fixture;

    private List<Data.Entity.Product> entities;

    public DeleteProductTests(BaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        // Seed data
        entities = new List<Data.Entity.Product>()
        {
            new Data.Entity.Product() { Name = "Banana" },
            new Data.Entity.Product() { Name = "Orange" },
            new Data.Entity.Product() { Name = "Grapes" }
        };
        await _fixture.AddRangeAsync(entities);
    }

    [Fact]
    public async Task Delete_With_Valid_Id_Is_Possible()
    {
        // Assert
        var command = new DeleteProduct() { Id = entities[0].Id };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_With_Invalid_Id_Is_Not_Possible()
    {
        // Assert
        var command = new DeleteProduct() { Id = 0 };

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
