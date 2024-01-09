using RedBlackTreeVisualizer.Models;

namespace RedBlackTreeVisualizer.Tests.UnitTesting.xUnit
{
    public class ValidatorTests
    {
        [Theory]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("1000")]
        public void IsIdValid_ReturnsTrue(string inputLine)
        {
            // Act
            var isValid = DataValidator.IsIdValid(inputLine);

            // Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("abc")]
        [InlineData("-1")]
        [InlineData("1.5")]
        public void IsIdValid_ReturnsFalse(string? inputLine)
        {
            // Act
            var isValid = DataValidator.IsIdValid(inputLine);

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("abc")]
        [InlineData("-1")]
        [InlineData("1,45,95")]
        [InlineData("-10.25")]
        [InlineData("5a")]
        public void IsCompensationValid_ReturnsFalse(string? inputLine)
        {
            // Act
            var isValid = DataValidator.IsCompensationValid(inputLine);

            // Assert
            Assert.False(isValid);
        }
    }
}
