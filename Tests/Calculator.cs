namespace Tests;

public class CalculatorTests
{
    [Theory]
    [InlineData(2, 2, 4)]
    [InlineData(0, 0, 0)]
    [InlineData(10, 10, 20)]
    public void SumMethod_Should_SumCorrectly(int a, int b, int expected)
    {
        // Arrange
        var calculator = new Calculator.Calculator();

        // Act
        var result = calculator.Sum(a, b);

        // Assert
        Assert.Equal(expected, result);
    }
}
