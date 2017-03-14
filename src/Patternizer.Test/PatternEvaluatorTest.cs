using System;
using System.Threading.Tasks;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using Patternizer;

namespace Patternizer.Test
{
	[TestFixture]
	public class PatternEvaluatorTest
	{

		[Test]
		public void WideRectanglePatternTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();

			// Act
			evaluator.Add("button").When(Pattern.WideRectangle);
			var result = evaluator.Evaluate(PathsForTesting.WideRectangle1);

			// Assert
			Assert.IsTrue(result.IsValid);
			Assert.AreEqual("button", result.Key);
		}

		[Test]
		public void WideRectanglePatternFailTest2()
		{
			// Arrange
			var evaluator = new PatternEvaluator();

			// Act
			evaluator.Add("button").When(Pattern.WideRectangle);
			var result = evaluator.Evaluate(PathsForTesting.TallRectangle);

			// Assert
			Assert.IsFalse(result.IsValid);
		}

		[Test]
		public void ImagePatternTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();

			// Act
			evaluator.Add("image").When(p => p.MovesRightAndDown())
								  .When(p => p.MovesLeftAndDown());

			var result = evaluator.Evaluate(PathsForTesting.ImageRectangle);

			// Assert
			Assert.IsTrue(result.IsValid);
			Assert.AreSame("image", result.Key);
		}

		[Test]
		public void WideRectanglePatternFailTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();

			// Act
			evaluator.Add("button").When(Pattern.WideRectangle);
			var result = evaluator.Evaluate(PathsForTesting.ImageRectangle);

			// Assert
			Assert.IsFalse(result.IsValid);
			Assert.IsNull(result.Key);
		}

		[Test]
		public void ImagePatternFailTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();

			// Act
			evaluator.Add("image").When(Pattern.WideRectangle)
				.When(p => p.MovesRightAndDown())
				.When(p => p.MovesLeftAndDown());
			var results = evaluator.Evaluate(PathsForTesting.WideRectangle1);

			// Assert
			Assert.IsFalse(results.IsValid);
		}

		[Test]
		public void ImagePatternWithMultipleRegisteredTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();

			// Act
			evaluator.Add("button").When(Pattern.WideRectangle);
			evaluator.Add("image")
				.When(p => p.MovesRightAndDown())
				.When(p => p.MovesLeftAndDown());

			var result = evaluator.Evaluate(PathsForTesting.ImageRectangle);

			// Assert
			Assert.IsTrue(result.IsValid);
			Assert.AreSame("image", result.Key);
		}

		[Test]
		public void WideRectangleBoundsTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();

			// Act
			evaluator.Add("button").When(Pattern.WideRectangle);
			var result = evaluator.Evaluate(PathsForTesting.WideRectangle1);

			// Assert
			Assert.IsTrue(result.IsValid);
			Assert.AreEqual("button", result.Key);
			Assert.AreEqual(PathsForTesting.WideRectangle1.Min(e => e.P1.X), result.UpperLeft.X);
		}

		[Test]
		public void JigSawPatternTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();

			// Act
			evaluator.Add("test").When(p => p.Repetitive(ip => ip.MovesRight().MovesLeftAndDown()).MovesRight());
			var result = evaluator.Evaluate(PathsForTesting.JigSawPath);

			// Assert
			Assert.IsTrue(result.IsValid);
			Assert.AreEqual("test", result.Key);
		}

		[Test]
		public void CrossPathTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();
			var path = Path.Parse("10,300 -> 100,200 | 100,300 -> 10,200");

			// Act
			evaluator.Add("test").When(Pattern.Cross);
			var result = evaluator.Evaluate(path);

			// Assert
			Assert.IsTrue(result.IsValid);
		}

		[Test]
		public void EntryTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();
			var path = Path.Parse("10,300 -> 10,270 -> 300,270 -> 300,300");

			// Act
			evaluator.Add("entry").When(p => p.MovesDown().MovesRight().MovesUp().Bounds(BoundsDescriptor.IsWide));
			var result = evaluator.Evaluate(path);

			// Assert
			Assert.IsTrue(result.IsValid);
		}
	}
}
