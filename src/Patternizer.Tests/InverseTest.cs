using Patternizer.Test;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Patternizer.Tests
{
    public class InverseTest
    {
        [Fact]
        public void InverseHorizontalTest()
        {
            // Arrange
            var evaluator = new PatternEvaluator();

            // Act
            evaluator.Add("image").When(p => p.MovesRightAndDown().MovesLeftAndDown()).AllowInverse(InverseDescriptor.Horizontal);
            var result = evaluator.Evaluate(PathsForTesting.InverseImage);

            // Assert
            Assert.True(result.IsValid);
            Assert.Same("image", result.Key);
        }
    }
}
