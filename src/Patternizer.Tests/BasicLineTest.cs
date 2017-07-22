using System;
using Patternizer;
using Xunit;

namespace Patternizer.Test
{
	public class BasicLineTest
	{
		[Fact]
		public void LineDownTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();
			var path = Path.Parse("10,300 -> 10,200");

			// Act
			evaluator.Add("linedown").When(e=>e.MovesDown());
			var result = evaluator.Evaluate(path);

			// Assert
			Assert.True(result.IsValid);
			Assert.Equal("linedown", result.Key);
		}
	}
}
