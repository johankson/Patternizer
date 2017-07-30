using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Patternizer.Tests
{
    public class WildCardTest
    {
        [Fact]
        public void AllowAnyOrderTest()
        {
            // Arrange
            var evaluator = new PatternEvaluator();
            var path1 = Path.Parse("300,300 -> 400,200 -> 200,200 -> 300,300"); // the same triangle, just drawn in different order
            var path2 = Path.Parse("400,200 -> 200,200 -> 300,300 -> 400,200");

            // Act
            evaluator.Add("allowanyorder-triangle").When(
                p => p.MovesRightAndDown()
                .MovesLeft()
                .MovesRightAndUp()
                .End(RelativePosition.NearStart)).AllowAnyOrder();
            var result1 = evaluator.Evaluate(path1);
            var result2 = evaluator.Evaluate(path2);

            // Assert
            Assert.True(result1.IsValid);
            Assert.True(result2.IsValid);
        }
    }
}
