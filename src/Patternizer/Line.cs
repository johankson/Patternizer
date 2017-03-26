using System;

namespace Patternizer
{
	public struct Line
	{
		public Point P1 { get; set; }
		public Point P2 { get; set; }

		public Line(Point p1, Point p2)
		{
			P1 = p1;
			P2 = p2;
		}

		public Line(float p1x, float p1y, float p2x, float p2y)
		{
			P1 = new Point(p1x, p1y);
			P2 = new Point(p2x, p2y);
		}

		public float Length
		{
			get
			{
				var dx = P2.X - P1.X;
				var dy = P2.Y - P1.Y;

				return (float)Math.Sqrt((dx * dx) + (dy * dy));
			}
		}

		public override string ToString()
		{
			return string.Format("[Line: P1={0}, P2={1}, Length={2}]", P1, P2, Length);
		}
	}
}
