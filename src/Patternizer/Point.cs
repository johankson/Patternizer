using System;
namespace Patternizer
{
	public struct Point
	{
		public float X { get; set; }
		public float Y { get; set; }

		public Point(float x, float y)
		{
			X = x;
			Y = y;
		}

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public double Magnitude
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y);
            }
        }

	}
}
