using StringDataSorter.Core.Comparer;
using Xunit;

namespace StringDataSorter.Tests
{
    public class StringDataComparerTests
    {
        private readonly StringDataComparer _comparer = new StringDataComparer();

        [Fact]
        public void Compare_SameReference_ShouldReturnZero()
        {
            // Arrange
            var input = "100. Apple";

            // Act
            int result = _comparer.Compare(input, input);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Compare_BothNull_ShouldReturnZero()
        {
            // Act
            int result = _comparer.Compare(null!, null!);

            // Assert
            // Expecting 0 if we decide "both null" are equal
            Assert.Equal(0, result);
        }

        [Fact]
        public void Compare_xIsNull_ShouldReturnNegativeOne()
        {
            // Arrange
            string x = null!;
            string y = "100. Apple";

            // Act
            int result = _comparer.Compare(x, y);

            // Assert
            // x is null, so it should return -1
            Assert.Equal(-1, result);
        }

        [Fact]
        public void Compare_yIsNull_ShouldReturnPositiveOne()
        {
            // Arrange
            string x = "100. Apple";
            string y = null!;

            // Act
            int result = _comparer.Compare(x, y);

            // Assert
            // y is null, so it should return 1
            Assert.Equal(1, result);
        }

        [Theory]
        [InlineData("101. Apple", "101. Apple", 0)]
        [InlineData("101. Apple", "102. Apple", -1)]  
        [InlineData("202. Apple", "201. Apple", 1)] 
        public void Compare_SameTextDifferentNumbers_ReturnsCorrectComparison(string x, string y, int expected)
        {
            // Act
            int result = _comparer.Compare(x, y);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("100. Apple", "100. Banana", -1)]  
        [InlineData("100. Banana", "100. Apple", 1)]   
        [InlineData("100. Banana", "100. Banana", 0)]  
        public void Compare_DifferentText_ReturnsCorrectComparison(string x, string y, int expected)
        {
            // Act
            int result = _comparer.Compare(x, y);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Compare_EdgeCase_InvalidNumericPart_FallsBackToStringCompare()
        {
            // Arrange
            // One of them has a non-numeric prefix, which can't parse to int
            string x = "XYZ. Apple";
            string y = "123. Apple";

            // Act
            int result = _comparer.Compare(x, y);

            // Assert
            // Whne it tries to parse the numbers and fails, fall back to full string compare
            Assert.True(result != 0, "Expected a non-zero result because fallback string compare should differ.");
        }
    }
}
