using PriceTracker.Data.Store.Commands;

namespace PriceTracker.Testing.Store.Commands;

public class CreateStoreTests : IClassFixture<BaseTestFixture>, IAsyncLifetime
{
    private readonly BaseTestFixture _fixture;

    public CreateStoreTests(BaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync() => Task.CompletedTask;


    [Fact]
    public async Task Create_With_Valid_Values_Is_Possible()
    {
        // Arrange        
        var createCommand = new CreateStore() { Name = "Target" };

        // Act
        var result = await _fixture.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be(createCommand.Name);
    }

    [Fact]
    public async Task Create_With_Duplicate_Name_Is_Not_Possible()
    {
        // Arrange 
        await _fixture.AddAsync(new Data.Entity.Store() { Name = "Target" });
        var createCommand = new CreateStore() { Name = "Target" };

        // Act
        var result = await _fixture.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public async Task Create_Without_Name_Is_Not_Possible()
    {
        // Arrange
        var createCommand = new CreateStore() { Name = "" };

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
