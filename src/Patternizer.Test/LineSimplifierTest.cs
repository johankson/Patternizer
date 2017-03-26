using System;
using NUnit.Framework;

namespace Patternizer.Test
{
	[TestFixture]
	public class LineSimplifierTest
	{
		[Test]
		public void BrokenLineTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();
			var path = Path.Parse("10,300 -> 20,320 -> 100,310");

			// Act
			evaluator.Add("line").When(e => e.MovesRight());
			var result = evaluator.Evaluate(path);

			// Assert
			Assert.IsTrue(result.IsValid);
			Assert.AreEqual("line", result.Key);
		}
	}
}
