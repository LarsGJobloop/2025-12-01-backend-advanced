namespace Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var a = 2;
        var b = 2;
        var expected = 5;

        // Act
        var result = a + b;

        // Assert
        Assert.Equal(expected, result);
    }
}
