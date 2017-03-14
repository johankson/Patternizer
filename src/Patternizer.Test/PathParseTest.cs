using System;
using NUnit.Framework;
using Patternizer;

namespace Patternizer.Test
{
	[TestFixture]
	public class PathParseTest
	{

		[Test]
		public void ParseTest()
		{
			// Arrange
			var path = "10,10 -> 10,40 -> 30,40";

			// Act
			var list = Path.Parse(path);

			// Assert
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(10.0f, list[0].P1.X);
			Assert.AreEqual(10.0f, list[0].P1.Y);
			Assert.AreEqual(10.0f, list[0].P2.X);
			Assert.AreEqual(40.0f, list[0].P2.Y);
			Assert.AreEqual(30.0f, list[1].P2.X);
			Assert.AreEqual(40.0f, list[1].P2.Y);
		}

		[Test]
		public void ParseRowBasedTest()
		{
			// Arrange
			var path = "Line 331, 1058-> 485, 1058\nLine 488, 1058-> 503, 936";

			// Act
			var list = Path.Parse(path);

			// Assert
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(331f, list[0].P1.X);
		
		}
	}
}
