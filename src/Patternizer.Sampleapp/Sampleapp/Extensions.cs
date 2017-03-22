using System;
using CocosSharp;
using Patternizer;

namespace Sampleapp
{
	public static class Extensions
	{
		public static CCPoint ToCCPoint(this Point point)
		{
			return new CCPoint (point.X, point.Y);
		}
	}
}