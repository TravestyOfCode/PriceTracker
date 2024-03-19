using PriceTracker.Data.UnitConversion.Commands;
using System.Collections.Generic;

namespace PriceTracker.Testing.UnitConversion.Commands;

public class UpdateUnitConversionTests : IAsyncLifetime, IClassFixture<BaseTestFixture>
{
    private readonly BaseTestFixture _fixture;

    private Data.Entity.UnitConversion _conversion;

    private readonly List<Data.Entity.UnitOfMeasure> _uoms = new List<Data.Entity.UnitOfMeasure>()
    {
        new Data.Entity.UnitOfMeasure() { Name = "milliliter", Abbreviation = "ml", ConversionToGramsRatio = 1.0m },
        new Data.Entity.UnitOfMeasure() { Name = "ounce", Abbreviation = "oz", ConversionToGramsRatio = 28.35m }
    };

    public UpdateUnitConversionTests(BaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        await _fixture.AddRangeAsync(_uoms);
        _conversion = new Data.Entity.UnitConversion() { SourceUnitOfMeasureId = _uoms[0].Id, DestinationUnitOfMeasureId = _uoms[1].Id, ConversionRatio = 1.0m };
        await _fixture.AddAsync(_conversion);
    }

    [Fact]
    public async Task Update_With_Valid_Values_Is_Possible()
    {
        // Arrange
        var command = new UpdateUnitConversion() { Id = _conversion.Id, SourceUnitOfMeasureId = _uoms[1].Id, DestinationUnitOfMeasureId = _uoms[0].Id, ConversionRatio = 2.0m };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.SourceUnitOfMeasureId.Should().Be(command.SourceUnitOfMeasureId);
        result.Value.SourceUnitOfMeasure.Should().NotBeNull();
        result.Value.SourceUnitOfMeasure.Name.Should().Be(_uoms[1].Name);
        result.Value.DestinationUnitOfMeasureId.Should().Be(command.DestinationUnitOfMeasureId);
        result.Value.DestinationUnitOfMeasure.Should().NotBeNull();
        result.Value.DestinationUnitOfMeasure.Name.Should().Be(_uoms[0].Name);
        result.Value.ConversionRatio.Should().Be(command.ConversionRatio);

    }

    [Fact]
    public async Task Update_With_Duplicate_Source_And_Destination_Is_Not_Possible()
    {
        // Arrange        
        await _fixture.AddAsync(new Data.Entity.UnitConversion() { SourceUnitOfMeasureId = _uoms[1].Id, DestinationUnitOfMeasureId = _uoms[0].Id, ConversionRatio = 1m });
        var command = new UpdateUnitConversion() { Id = _conversion.Id, SourceUnitOfMeasureId = _uoms[1].Id, DestinationUnitOfMeasureId = _uoms[0].Id, ConversionRatio = 5m };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasFailure.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public async Task Update_With_Negative_ConversionRatio_Is_Not_Possible()
    {
        // Arrange
        var command = new UpdateUnitConversion() { Id = _conversion.Id, SourceUnitOfMeasureId = _uoms[0].Id, DestinationUnitOfMeasureId = _uoms[1].Id, ConversionRatio = -5m };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasFailure.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public async Task Update_With_Zero_ConversionRatio_Is_Not_Possible()
    {
        // Arrange
        var command = new UpdateUnitConversion() { Id = _conversion.Id, SourceUnitOfMeasureId = _uoms[0].Id, DestinationUnitOfMeasureId = _uoms[1].Id, ConversionRatio = 0m };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasFailure.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public async Task Update_With_Invalid_SourceUnitOfMeasureId_Is_Not_Possible()
    {
        // Arrange
        var command = new UpdateUnitConversion() { Id = _conversion.Id, SourceUnitOfMeasureId = 0, DestinationUnitOfMeasureId = _uoms[1].Id, ConversionRatio = 5m };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasFailure.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public async Task Update_With_Invalid_DestinationUnitOfMeasureId_Is_Not_Possible()
    {
        // Arrange
        var command = new UpdateUnitConversion() { Id = _conversion.Id, SourceUnitOfMeasureId = _uoms[0].Id, DestinationUnitOfMeasureId = 0, ConversionRatio = 5m };

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
