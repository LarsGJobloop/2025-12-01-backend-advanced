namespace Tests;

public class CalculatorTests
{
    [Fact]
    public void SumMethod_Should_SumCorrectly()
    {
        // Arrange
        var a = 2;
        var b = 2;
        var expected = 4;
        var calculator = new Calculator.Calculator();

        // Act
        var result = calculator.Sum(a, b);

        // Assert
        Assert.Equal(expected, result);
    }
}
