using PriceTracker.Data.Product.Commands;

namespace PriceTracker.Testing.Product.Commands;

public class CreateProductTests : IClassFixture<BaseTestFixture>, IAsyncLifetime
{
    private readonly BaseTestFixture _fixture;

    private readonly Data.Entity.UnitOfMeasure _ouncesUoM = new Data.Entity.UnitOfMeasure() { Name = "ounce", Abbreviation = "oz" };

    public CreateProductTests(BaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        await _fixture.AddAsync(_ouncesUoM);
    }

    [Fact]
    public async Task Create_With_Valid_Values_Is_Possible()
    {
        // Arrange        
        var createCommand = new CreateProduct() { Name = "Banana", DefaultUnitOfMeasureId = _ouncesUoM.Id };

        // Act
        var result = await _fixture.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be(createCommand.Name);
        result.Value.DefaultUnitOfMeasureId.Should().Be(_ouncesUoM.Id);
    }

    [Fact]
    public async Task Create_With_Duplicate_Name_Is_Not_Possible()
    {
        // Arrange 
        await _fixture.AddAsync(new Data.Entity.Product() { Name = "Banana", DefaultUnitOfMeasureId = _ouncesUoM.Id });
        var createCommand = new CreateProduct() { Name = "Banana" };

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
        var createCommand = new CreateProduct() { Name = "", DefaultUnitOfMeasureId = _ouncesUoM.Id };

        // Act
        var result = await _fixture.SendAsync(createCommand);

        // Assert
        result.WasSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_With_Invalid_DefaultUnitOfMeasure_Is_Not_Possible()
    {
        // Arrange
        var createCommand = new CreateProduct() { Name = "Banana", DefaultUnitOfMeasureId = -1 };

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
