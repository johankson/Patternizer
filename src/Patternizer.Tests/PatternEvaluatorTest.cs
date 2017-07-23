using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Patternizer;
using Xunit;

namespace Patternizer.Test
{
	public class PatternEvaluatorTest
	{
		[Fact]
		public void WideRectanglePatternTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();

			// Act
			evaluator.Add("button").When(Pattern.WideRectangle);
			var result = evaluator.Evaluate(PathsForTesting.WideRectangle1);

			// Assert
			Assert.True(result.IsValid);
			Assert.Equal("button", result.Key);
		}

		[Fact]
		public void WideRectanglePatternFailTest2()
		{
			// Arrange
			var evaluator = new PatternEvaluator();

			// Act
			evaluator.Add("button").When(Pattern.WideRectangle);
			var result = evaluator.Evaluate(PathsForTesting.TallRectangle);

			// Assert
			Assert.False(result.IsValid);
		}

		[Fact]
		public void ImagePatternTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();

			// Act
			evaluator.Add("image").When(p => p.MovesRightAndDown())
								  .When(p => p.MovesLeftAndDown());

			var result = evaluator.Evaluate(PathsForTesting.Image);

			// Assert
			Assert.True(result.IsValid);
			Assert.Same("image", result.Key);
		}

		[Fact]
		public void WideRectanglePatternFailTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();

			// Act
			evaluator.Add("button").When(Pattern.WideRectangle);
			var result = evaluator.Evaluate(PathsForTesting.Image);

			// Assert
			Assert.False(result.IsValid);
			Assert.Null(result.Key);
		}

		[Fact]
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
			Assert.False(results.IsValid);
		}

		[Fact]
		public void ImagePatternWithMultipleRegisteredTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();

			// Act
			evaluator.Add("button").When(Pattern.WideRectangle);
			evaluator.Add("image")
				.When(p => p.MovesRightAndDown())
				.When(p => p.MovesLeftAndDown());

			var result = evaluator.Evaluate(PathsForTesting.Image);

			// Assert
			Assert.True(result.IsValid);
			Assert.Same("image", result.Key);
		}

		[Fact]
		public void WideRectangleBoundsTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();

			// Act
			evaluator.Add("button").When(Pattern.WideRectangle);
			var result = evaluator.Evaluate(PathsForTesting.WideRectangle1);

			// Assert
			Assert.True(result.IsValid);
			Assert.Equal("button", result.Key);
			Assert.Equal(PathsForTesting.WideRectangle1.Min(e => e.P1.X), result.UpperLeft.X);
		}

		[Fact]
		public void JigSawPatternTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();

			// Act
			evaluator.Add("test").When(p => p.Repetitive(ip => ip.MovesRight().MovesLeftAndDown()).MovesRight());
			var result = evaluator.Evaluate(PathsForTesting.JigSawPath);

			// Assert
			Assert.True(result.IsValid);
			Assert.Equal("test", result.Key);
		}

		[Fact]
		public void CrossPathTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();
			var path = Path.Parse("10,300 -> 100,200 | 100,300 -> 10,200");

			// Act
			evaluator.Add("test").When(Pattern.Cross);
			var result = evaluator.Evaluate(path);

			// Assert
			Assert.True(result.IsValid);
		}

		[Fact]
		public void EntryTest()
		{
			// Arrange
			var evaluator = new PatternEvaluator();
			var path = Path.Parse("10,300 -> 10,260 -> 600,260 -> 600,300");

			// Act
			evaluator.Add("entry").When(p => p.MovesDown().MovesRight().MovesUp().Bounds(BoundsDescriptor.IsWide));
			var result = evaluator.Evaluate(path);

			// Assert
			Assert.True(result.IsValid);
		}
	}
}
