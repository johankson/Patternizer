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
	}
}
