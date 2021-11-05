﻿namespace HyperCacheUnitTests
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
    }
}
