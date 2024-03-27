using PriceTracker.Data.Results;
using PriceTracker.Web.Utilities;
using System.Collections.Generic;

namespace PriceTracker.Testing.Pagination;

public class PageUtilitiesTests
{
    private class MockIPageQuery : IPagedQuery
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public string SortBy { get; set; }
        public SortOrder SortOrder { get; set; }
    }

    private MockIPageQuery mockQuery;

    public PageUtilitiesTests()
    {
        mockQuery = new MockIPageQuery() { Page = 1, PerPage = 10 };

    }

    [Fact]
    public void StartPage_With_Current_Page_1_Calculates_Correctly()
    {
        // Arrange
        mockQuery.Page = 1;
        var result = PagedResult<int>.Ok(new List<int>(), mockQuery, 100);

        // Act

        // Assert
        result.StartPage().Should().Be(1);
    }

    [Fact]
    public void StartPage_With_Current_Page_2_Calculates_Correctly()
    {
        // Arrange
        mockQuery.Page = 2;
        var result = PagedResult<int>.Ok(new List<int>(), mockQuery, 100);

        // Act

        // Assert
        result.StartPage().Should().Be(1);
    }

    [Fact]
    public void StartPage_With_Current_Page_4_Calculates_Correctly()
    {
        // Arrange
        mockQuery.Page = 4;
        var result = PagedResult<int>.Ok(new List<int>(), mockQuery, 100);

        // Act

        // Assert
        result.StartPage().Should().Be(2);
    }

    [Fact]
    public void StartPage_With_Current_Page_8_Calculates_Correctly()
    {
        // Arrange
        mockQuery.Page = 8;
        var result = PagedResult<int>.Ok(new List<int>(), mockQuery, 100);

        // Act

        // Assert
        result.StartPage().Should().Be(6);
    }

    [Fact]
    public void StartPage_With_Current_Page_10_Calculates_Correctly()
    {
        // Arrange
        mockQuery.Page = 10;
        var result = PagedResult<int>.Ok(new List<int>(), mockQuery, 100);

        // Act

        // Assert
        result.StartPage().Should().Be(6);
    }

    [Fact]
    public void EndPage_With_Current_Page_1_Calculates_Correctly()
    {
        // Arrange
        mockQuery.Page = 1;
        var result = PagedResult<int>.Ok(new List<int>(), mockQuery, 100);

        // Act

        // Assert
        result.EndPage().Should().Be(5);
    }

    [Fact]
    public void EndPage_With_Current_Page_2_Calculates_Correctly()
    {
        // Arrange
        mockQuery.Page = 2;
        var result = PagedResult<int>.Ok(new List<int>(), mockQuery, 100);

        // Act

        // Assert
        result.EndPage().Should().Be(5);
    }

    [Fact]
    public void EndPage_With_Current_Page_4_Calculates_Correctly()
    {
        // Arrange
        mockQuery.Page = 4;
        var result = PagedResult<int>.Ok(new List<int>(), mockQuery, 100);

        // Act

        // Assert
        result.EndPage().Should().Be(6);
    }

    [Fact]
    public void EndPage_With_Current_Page_8_Calculates_Correctly()
    {
        // Arrange
        mockQuery.Page = 8;
        var result = PagedResult<int>.Ok(new List<int>(), mockQuery, 100);

        // Act

        // Assert
        result.EndPage().Should().Be(10);
    }

    [Fact]
    public void EndPage_With_Current_Page_10_Calculates_Correctly()
    {
        // Arrange
        mockQuery.Page = 10;
        var result = PagedResult<int>.Ok(new List<int>(), mockQuery, 100);

        // Act

        // Assert
        result.EndPage().Should().Be(10);
    }
}
