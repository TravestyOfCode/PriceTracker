using PriceTracker.Data.UnitConversion.Commands;
using System.Collections.Generic;

namespace PriceTracker.Testing.UnitConversion.Commands;

public class DeleteUnitConversionTests : IAsyncLifetime, IClassFixture<BaseTestFixture>
{
    private readonly BaseTestFixture _fixture;

    private List<Data.Entity.UnitOfMeasure> _uoms;

    private Data.Entity.UnitConversion _conversion;

    public DeleteUnitConversionTests(BaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        // Seed data
        _uoms = new List<Data.Entity.UnitOfMeasure>()
        {
            new Data.Entity.UnitOfMeasure(){ Name = "ounce", Abbreviation = "oz", ConversionToGramsRatio = 5m },
            new Data.Entity.UnitOfMeasure(){Name= "pound", Abbreviation = "lbs", ConversionToGramsRatio = 1m }
        };
        await _fixture.AddRangeAsync(_uoms);

        _conversion = new Data.Entity.UnitConversion() { SourceUnitOfMeasureId = _uoms[0].Id, DestinationUnitOfMeasureId = _uoms[1].Id, ConversionRatio = 1.0m };
        await _fixture.AddAsync(_conversion);
    }

    [Fact]
    public async Task Delete_With_Valid_Id_Is_Possible()
    {
        // Assert
        var command = new DeleteUnitConversion() { Id = _conversion.Id };

        // Act
        var result = await _fixture.SendAsync(command);

        // Assert
        result.WasSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_With_Invalid_Id_Is_Not_Possible()
    {
        // Assert
        var command = new DeleteUnitConversion() { Id = 0 };

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
