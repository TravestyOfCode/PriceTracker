using PriceTracker.Data.UnitOfMeasure.Commands;
using System.Collections.Generic;

namespace PriceTracker.Testing.UnitOfMeasure.Commands;

public class UpdateUnitOfMeasureTests : IAsyncLifetime, IClassFixture<BaseTestFixture>
{
    private readonly BaseTestFixture _fixture;

    private List<Data.Entity.UnitOfMeasure> entities;

    public UpdateUnitOfMeasureTests(BaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        // Seed data
        entities = new List<Data.Entity.UnitOfMeasure>()
        {
            new Data.Entity.UnitOfMeasure() { Name = "ounces", Abbreviation = "ozs", ConversionToGramsRatio = 1.0m },
            new Data.Entity.UnitOfMeasure() { Name = "pounds", Abbreviation = "lbs", ConversionToGramsRatio = 1.0m },
            new Data.Entity.UnitOfMeasure() { Name = "grams", Abbreviation = "gs", ConversionToGramsRatio = 1.0m }
        };
        await _fixture.AddRangeAsync(entities);
    }

    [Fact]
    public async Task Update_With_Valid_Values_Is_Possible()
    {
        // Arrange
        var command = new UpdateUnitOfMeasure() { Id = entities[0].Id, Name = "ounce", Abbreviation = "oz", ConversionToGramsRatio = 28.5m };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be(command.Name);
        result.Value.Abbreviation.Should().Be(command.Abbreviation);
        result.Value.ConversionToGramsRatio.Should().Be(command.ConversionToGramsRatio);
    }

    [Fact]
    public async Task Update_With_Duplicate_Name_Is_Not_Possible()
    {
        // Arrange
        var command = new UpdateUnitOfMeasure() { Id = entities[1].Id, Name = "grams", Abbreviation = "g", ConversionToGramsRatio = 1.0m };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_With_Duplicate_Abbreviation_Is_Not_Possible()
    {
        // Arrange
        var command = new UpdateUnitOfMeasure() { Id = entities[1].Id, Name = "gram", Abbreviation = "gs", ConversionToGramsRatio = 1.0m };

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
        var command = new UpdateUnitOfMeasure() { Id = entities[1].Id, Name = "", Abbreviation = "lb", ConversionToGramsRatio = 1.0m };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_With_Empty_Abbreviation_Is_Not_Possible()
    {
        // Arrange
        var command = new UpdateUnitOfMeasure() { Id = entities[1].Id, Name = "pound", Abbreviation = "", ConversionToGramsRatio = 1.0m };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_With_Negative_Conversion_Ratio_Is_Not_Possible()
    {
        // Arrange
        var command = new UpdateUnitOfMeasure() { Id = entities[1].Id, Name = "pound", Abbreviation = "lb", ConversionToGramsRatio = -1.0m };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_With_Zero_Conversion_Ratio_Is_Not_Possible()
    {
        // Arrange
        var command = new UpdateUnitOfMeasure() { Id = entities[1].Id, Name = "pound", Abbreviation = "lb", ConversionToGramsRatio = 0.0m };

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
        var command = new UpdateUnitOfMeasure() { Id = 0, Name = "pound", Abbreviation = "lb", ConversionToGramsRatio = 1.0m };

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
