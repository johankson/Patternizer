using System;
using Patternizer;
using Xunit;

namespace Patternizer.Test
{
	public class PathParseTest
	{

		[Fact]
		public void ParseTest()
		{
			// Arrange
			var path = "10,10 -> 10,40 -> 30,40";

			// Act
			var list = Path.Parse(path);

			// Assert
			Assert.Equal(2, list.Count);
			Assert.Equal(10.0f, list[0].P1.X);
			Assert.Equal(10.0f, list[0].P1.Y);
			Assert.Equal(10.0f, list[0].P2.X);
			Assert.Equal(40.0f, list[0].P2.Y);
			Assert.Equal(30.0f, list[1].P2.X);
			Assert.Equal(40.0f, list[1].P2.Y);
		}

		[Fact]
		public void ParseRowBasedTest()
		{
			// Arrange
			var path = "Line 331, 1058-> 485, 1058\nLine 488, 1058-> 503, 936";

			// Act
			var list = Path.Parse(path);

			// Assert
			Assert.Equal(2, list.Count);
			Assert.Equal(331f, list[0].P1.X);
		}
	}
}
