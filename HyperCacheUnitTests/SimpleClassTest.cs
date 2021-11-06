namespace HyperCacheUnitTests
{
    using AutoFixture;
    using FluentAssertions;
    using ProjectExample.SimpleTest;
    using Xunit;

    public class SimpleClassTest : BaseTest
    {
        [Fact]
        public void TestMethodWithouthParams()
        {
            // Arrange
            ISimpleClass simpleClass = new SimpleClass();
            var expectedResult = simpleClass.IntReturnMethod();

            // Act
            var result = simpleClass.IntReturnMethod();

            // Assert
            result.Should().Be(expectedResult);
          }

        [Fact]
        public void TestMethodWithIntParam()
        {
            // Arrange
            var param1 = this.Fixture.Create<int>();

            ISimpleClass simpleClass = new SimpleClass();
            var expectedResult = simpleClass.IntReturnMethod(param1);

            // Act
            var result = simpleClass.IntReturnMethod(param1);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void TestMethodWithIntAndStringParam()
        {
            // Arrange
            var param1 = this.Fixture.Create<int>();
            var param2 = this.Fixture.Create<string>();

            ISimpleClass simpleClass = new SimpleClass();
            var expectedResult = simpleClass.IntReturnMethod(param1, param2);

            // Act
            var result = simpleClass.IntReturnMethod(param1, param2);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void TestMethodWithNullableIntAndStringParam()
        {
            // Arrange
            int? param1 = null;
            var param2 = this.Fixture.Create<string>();

            ISimpleClass simpleClass = new SimpleClass();
            var expectedResult = simpleClass.IntReturnMethod(param1, param2);

            // Act
            var result = simpleClass.IntReturnMethod(param1, param2);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void TestMethodWithStringAndNullableIntParam()
        {
            // Arrange
            var param1 = this.Fixture.Create<string>();
            int? param2 = null;

            ISimpleClass simpleClass = new SimpleClass();
            var expectedResult = simpleClass.IntReturnMethod(param1, param2);

            // Act
            var result = simpleClass.IntReturnMethod(param1, param2);

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}
