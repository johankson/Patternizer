using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Patternizer.Tests
{
    public class StepTest
    {
        [Fact]
        public void Step_MovesLeftAndUpStep_Test()
        {
            // Arrange
            var evaluator = new PatternEvaluator();
            var path = Path.Parse("300,200 -> 200,300");

            // Act
            evaluator.Add("p").When(e => e.MovesLeftAndUp());
            var result = evaluator.Evaluate(path);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal("p", result.Key);
        }

        [Fact]
        public void Step_MovesLeftAndDownStep_Test()
        {
            // Arrange
            var evaluator = new PatternEvaluator();
            var path = Path.Parse("300,300 -> 200,200");

            // Act
            evaluator.Add("p").When(e => e.MovesLeftAndDown());
            var result = evaluator.Evaluate(path);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal("p", result.Key);
        }

        [Fact]
        public void Step_EndsOnRightSideStep_Test()
        {
            // Arrange
            var evaluator = new PatternEvaluator();
            var path = Path.Parse("200,200 -> 50,200 -> 197, 200");

            // Act
            evaluator.Add("p").When(e => e.MovesLeft().MovesRight().End(RelativePosition.NearRightSide));
            var result = evaluator.Evaluate(path);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
