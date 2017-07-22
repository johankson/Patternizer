using System;
using Xunit;

namespace Patternizer.Test
{
	public class LineSimplifierTest
	{
		[Fact]
		public void BrokenLineTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();
			var path = Path.Parse("10,300 -> 20,320 -> 100,310");

			// Act
			evaluator.Add("line").When(e => e.MovesRight());
			var result = evaluator.Evaluate(path);

			// Assert
			Assert.True(result.IsValid);
			Assert.Equal("line", result.Key);
		}
	}
}
