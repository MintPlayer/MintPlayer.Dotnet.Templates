namespace MintPlayer.Dotnet.WebApplication.Tests;

public class SampleTests
{
    [Fact]
    public void Sample_Test_Passes()
    {
        // Arrange
        var expected = 4;

        // Act
        var actual = 2 + 2;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(5, 5, 10)]
    [InlineData(-1, 1, 0)]
    public void Addition_Returns_Correct_Sum(int a, int b, int expected)
    {
        // Act
        var actual = a + b;

        // Assert
        Assert.Equal(expected, actual);
    }
}
