using PriceTracker.Data.Store.Commands;
using System.Collections.Generic;

namespace PriceTracker.Testing.Store.Commands;

public class DeleteStoreTests : IAsyncLifetime, IClassFixture<BaseTestFixture>
{
    private readonly BaseTestFixture _fixture;

    private List<Data.Entity.Store> entities;

    public DeleteStoreTests(BaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        // Seed data
        entities = new List<Data.Entity.Store>()
        {
            new Data.Entity.Store() { Name = "Target" },
            new Data.Entity.Store() { Name = "Walmart" },
            new Data.Entity.Store() { Name = "Amazon" }
        };
        await _fixture.AddRangeAsync(entities);
    }

    [Fact]
    public async Task Delete_With_Valid_Id_Is_Possible()
    {
        // Assert
        var command = new DeleteStore() { Id = entities[0].Id };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_With_Invalid_Id_Is_Not_Possible()
    {
        // Assert
        var command = new DeleteStore() { Id = 0 };

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
