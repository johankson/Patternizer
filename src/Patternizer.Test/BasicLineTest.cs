using System;
using NUnit.Framework;
using Patternizer;

namespace Patternizer.Test
{
	[TestFixture]
	public class BasicLineTest
	{

		[Test]
		public void LineDownTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();
			var path = Path.Parse("10,300 -> 10,200");

			// Act
			evaluator.Add("linedown").When(e=>e.MovesDown());
			var result = evaluator.Evaluate(path);

			// Assert
			Assert.IsTrue(result.IsValid);
			Assert.AreEqual("linedown", result.Key);
		}
	}
}
