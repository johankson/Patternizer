using System;
using System.Collections.Generic;
using CocosSharp;
using System.Linq;

namespace Sampleapp.Shapes
{
	public abstract class Shape
	{
		public const float LINE_THICKNESS = 7f;
		public abstract void Draw (CCDrawNode node);
		public abstract CCRect Bounds();

		/// <summary>
		/// Calculates the bounds of a list of points
		/// </summary>
		/// <returns>The bounds.</returns>
		/// <param name="points">Points.</param>
		public static CCRect CalculateBounds(IEnumerable<CCPoint> points)
		{
			var yMin = points.Min (e => e.Y);
			var yMax = points.Max (e => e.Y);
			var xMin = points.Min (e => e.X);
			var xMax = points.Max (e => e.X);

			return new CCRect (xMin, yMin, xMax-xMin, yMax-yMin);
		}

		public static CCRect CalculateBounds(params CCPoint[] points)
		{
			return CalculateBounds (points.ToList());
		}
	}
}