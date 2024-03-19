using PriceTracker.Data.UnitConversion.Commands;
using System.Collections.Generic;

namespace PriceTracker.Testing.UnitConversion.Commands;

public class CreateUnitConversionTests : IAsyncLifetime, IClassFixture<BaseTestFixture>
{
    private readonly BaseTestFixture _fixture;

    private readonly List<Data.Entity.UnitOfMeasure> _uoms = new List<Data.Entity.UnitOfMeasure>()
    {
        new Data.Entity.UnitOfMeasure() { Name = "milliliter", Abbreviation = "ml", ConversionToGramsRatio = 1.0m },
        new Data.Entity.UnitOfMeasure() { Name = "ounce", Abbreviation = "oz", ConversionToGramsRatio = 28.35m }
    };

    public CreateUnitConversionTests(BaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        await _fixture.AddRangeAsync(_uoms);
    }

    [Fact]
    public async Task Create_With_Valid_Values_Is_Possible()
    {
        // Arrange
        var createCommand = new CreateUnitConversion() { SourceUnitOfMeasureId = _uoms[0].Id, DestinationUnitOfMeasureId = _uoms[1].Id, ConversionRatio = 30m };

        // Act
        var result = await _fixture.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.SourceUnitOfMeasureId.Should().Be(createCommand.SourceUnitOfMeasureId);
        result.Value.SourceUnitOfMeasure.Should().NotBeNull();
        result.Value.DestinationUnitOfMeasureId.Should().Be(createCommand.DestinationUnitOfMeasureId);
        result.Value.DestinationUnitOfMeasure.Should().NotBeNull();
        result.Value.ConversionRatio.Should().Be(createCommand.ConversionRatio);
    }

    [Fact]
    public async Task Create_With_Duplicate_Source_And_Destination_Is_Not_Possible()
    {
        // Arrange
        await _fixture.AddAsync(new Data.Entity.UnitConversion() { SourceUnitOfMeasureId = _uoms[0].Id, DestinationUnitOfMeasureId = _uoms[1].Id, ConversionRatio = 1m });
        var command = new CreateUnitConversion() { SourceUnitOfMeasureId = _uoms[0].Id, DestinationUnitOfMeasureId = _uoms[1].Id, ConversionRatio = 5m };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasFailure.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public async Task Create_With_Negative_ConversionRatio_Is_Not_Possible()
    {
        // Arrange
        var command = new CreateUnitConversion() { SourceUnitOfMeasureId = _uoms[0].Id, DestinationUnitOfMeasureId = _uoms[1].Id, ConversionRatio = -5m };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasFailure.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public async Task Create_With_Zero_ConversionRatio_Is_Not_Possible()
    {
        // Arrange
        var command = new CreateUnitConversion() { SourceUnitOfMeasureId = _uoms[0].Id, DestinationUnitOfMeasureId = _uoms[1].Id, ConversionRatio = 0m };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasFailure.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public async Task Create_With_Invalid_SourceUnitOfMeasureId_Is_Not_Possible()
    {
        // Arrange
        var command = new CreateUnitConversion() { SourceUnitOfMeasureId = 0, DestinationUnitOfMeasureId = _uoms[1].Id, ConversionRatio = 5m };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasFailure.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public async Task Create_With_Invalid_DestinationUnitOfMeasureId_Is_Not_Possible()
    {
        // Arrange
        var command = new CreateUnitConversion() { SourceUnitOfMeasureId = _uoms[0].Id, DestinationUnitOfMeasureId = 0, ConversionRatio = 5m };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasFailure.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        result.Errors.Should().HaveCount(1);
    }

    public async Task DisposeAsync()
    {
        await _fixture.ResetDatabase();
    }
}
