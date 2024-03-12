using PriceTracker.Data.UnitOfMeasure.Commands;

namespace PriceTracker.Testing.UnitOfMeasure.Commands;

public class CreateUnitOfMeasureTests : IAsyncLifetime, IClassFixture<BaseTestFixture>
{
    private readonly BaseTestFixture _scope;

    public CreateUnitOfMeasureTests(BaseTestFixture scope)
    {
        _scope = scope;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async Task CanAddNewUnitOfMeasure()
    {
        // Arrange        
        var createCommand = new CreateUnitOfMeasure() { Name = "ounce", Abbreviation = "oz", ConversionToGramsRatio = 28.5m };

        // Act
        var result = await _scope.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be(createCommand.Name);
        result.Value.Abbreviation.Should().Be(createCommand.Abbreviation);
        result.Value.ConversionToGramsRatio.Should().Be(createCommand.ConversionToGramsRatio);
    }

    [Fact]
    public async Task CanNotAddNewUnitOfMeasureWithDuplicateName()
    {
        // Arrange 
        await _scope.AddAsync(new Data.Entity.UnitOfMeasure() { Name = "ounce", Abbreviation = "o", ConversionToGramsRatio = 28.5m });
        var createCommand = new CreateUnitOfMeasure() { Name = "ounce", Abbreviation = "oz", ConversionToGramsRatio = 28.5m };

        // Act
        var result = await _scope.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public async Task CanNotAddNewUnitOfMeasureWithDuplicateAbbreviation()
    {
        // Arrange
        await _scope.AddAsync(new Data.Entity.UnitOfMeasure() { Name = "ounces", Abbreviation = "oz", ConversionToGramsRatio = 28.5m });
        var createCommand = new CreateUnitOfMeasure() { Name = "ounce", Abbreviation = "oz", ConversionToGramsRatio = 28.5m };

        // Act
        var result = await _scope.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public async Task CanNotAddNewUnitOfMeasureEmptyName()
    {
        // Arrange
        var createCommand = new CreateUnitOfMeasure() { Name = "", Abbreviation = "oz", ConversionToGramsRatio = 28.5m };

        // Act
        var result = await _scope.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
    }

    public async Task DisposeAsync()
    {
        await _scope.ResetDatabase();
    }
}
