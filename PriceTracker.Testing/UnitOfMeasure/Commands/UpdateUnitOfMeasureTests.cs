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
            new Data.Entity.UnitOfMeasure() { Name = "ounces", Abbreviation = "ozs" },
            new Data.Entity.UnitOfMeasure() { Name = "pounds", Abbreviation = "lbs" },
            new Data.Entity.UnitOfMeasure() { Name = "grams", Abbreviation = "gs" }
        };
        await _fixture.AddRangeAsync(entities);
    }

    [Fact]
    public async Task Update_With_Valid_Values_Is_Possible()
    {
        // Arrange
        var command = new UpdateUnitOfMeasure() { Id = entities[0].Id, Name = "ounce", Abbreviation = "oz" };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be(command.Name);
        result.Value.Abbreviation.Should().Be(command.Abbreviation);
    }

    [Fact]
    public async Task Update_With_Duplicate_Name_Is_Not_Possible()
    {
        // Arrange
        var command = new UpdateUnitOfMeasure() { Id = entities[1].Id, Name = "grams", Abbreviation = "g" };

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
        var command = new UpdateUnitOfMeasure() { Id = entities[1].Id, Name = "gram", Abbreviation = "gs" };

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
        var command = new UpdateUnitOfMeasure() { Id = entities[1].Id, Name = "", Abbreviation = "lb" };

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
        var command = new UpdateUnitOfMeasure() { Id = entities[1].Id, Name = "pound", Abbreviation = "" };

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
        var command = new UpdateUnitOfMeasure() { Id = 0, Name = "pound", Abbreviation = "lb" };

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
