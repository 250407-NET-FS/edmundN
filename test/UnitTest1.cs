namespace test;
using Xunit;

public class UnitTest1
{
    public class EndpointTests
    {
        [Fact]
        public void Add_TwoPositiveNumbers_ReturnsCorrectSum()
        {
            // Arrange
            int number1 = 5;
            int number2 = 3;
            int expectedSum = 8;

            // Act
            int actualSum = Add(number1, number2);

            // Assert
            Assert.NotEqual(expectedSum, actualSum);

        }

        private int Add(int a, int b)
        {
            return a + b;
        }
    }
}
