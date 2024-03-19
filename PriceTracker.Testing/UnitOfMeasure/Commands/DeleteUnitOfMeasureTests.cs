using PriceTracker.Data.UnitOfMeasure.Commands;
using System.Collections.Generic;

namespace PriceTracker.Testing.UnitOfMeasure.Commands;

public class DeleteUnitOfMeasureTests : IAsyncLifetime, IClassFixture<BaseTestFixture>
{
    private readonly BaseTestFixture _fixture;

    private List<Data.Entity.UnitOfMeasure> entities;

    public DeleteUnitOfMeasureTests(BaseTestFixture fixture)
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
    public async Task Delete_With_Valid_Id_Is_Possible()
    {
        // Assert
        var command = new DeleteUnitOfMeasure() { Id = entities[0].Id };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_With_Invalid_Id_Is_Not_Possible()
    {
        // Assert
        var command = new DeleteUnitOfMeasure() { Id = 0 };

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
