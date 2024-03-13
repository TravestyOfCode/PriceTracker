using PriceTracker.Data.Store.Commands;
using System.Collections.Generic;

namespace PriceTracker.Testing.Store.Commands;

public class UpdateStoreTests : IAsyncLifetime, IClassFixture<BaseTestFixture>
{
    private readonly BaseTestFixture _fixture;

    private List<Data.Entity.Store> stores;

    public UpdateStoreTests(BaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        // Seed data
        stores = new List<Data.Entity.Store>()
        {
            new Data.Entity.Store() { Name = "Target" },
            new Data.Entity.Store() { Name = "Walmart" },
            new Data.Entity.Store() { Name = "Amazon" }
        };
        await _fixture.AddRangeAsync(stores);
    }

    [Fact]
    public async Task Update_With_Valid_Values_Is_Possible()
    {
        // Arrange
        var command = new UpdateStore() { Id = stores[0].Id, Name = "Stop & Shop" };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be(command.Name);
    }

    [Fact]
    public async Task Update_With_Duplicate_Name_Is_Not_Possible()
    {
        // Arrange
        var command = new UpdateStore() { Id = stores[1].Id, Name = "Target" };

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
        var command = new UpdateStore() { Id = stores[1].Id, Name = "" };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_With_Invalid_Id_Is_Not_Possible()
    {
        // Arrange
        var command = new UpdateStore() { Id = 0, Name = "Stop & Shop" };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    public async Task DisposeAsync()
    {
        await _fixture.ResetDatabase();
    }

}
