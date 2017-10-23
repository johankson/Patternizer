using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Patternizer
{
	/// <summary>
	/// A simple Line Recognizer that tracks rapid input and resolves lines
	/// on the fly.
	/// </summary>
	public class LineRecognizer
	{
		/// <summary>
		/// The point log is a storage for all points passed to this class. Mainly used for debugging purposes.
		/// </summary>
		private List<Point> _pointLog;
		private List<Point> _points;
		private List<Point> _processedPoints;
		private List<Line> _lines;

		public Action<Line> LineFound;
		public Action<List<Line>> RecognitionEnded;

		public LineRecognizer()
		{
			Initialize();
		}

		private void Initialize()
		{
			_pointLog = new List<Point>();
			_points = new List<Point>();
			_processedPoints = new List<Point>();
			_lines = new List<Line>();
		}

		/// <summary>
		/// Gets a list of all points as they were registered since the
		/// recognition began.
		/// </summary>
		/// <value>A list of points</value>
		public List<Point> AllPoints
		{
			get { return _pointLog; }
		}

		public void Clear()
		{
			_pointLog.Clear();
			_lines.Clear();
			_points.Clear();

			_processedPoints.Clear();
		}

        public void RegisterPoints(IEnumerable<Point> points)
        {
            foreach (var point in points)
            {
                RegisterPoint(point);
            }
        }

		public void RegisterPoint(float x, float y)
		{
			RegisterPoint(new Point() { X = x, Y = y });
		}

		public void RegisterPoint(Point point)
		{
            Debug.WriteLine($"{point.X}, {point.Y}");
			_pointLog.Add(point);
			_points.Add(point);

			if (_points.Count < 2)
			{
				return;
			}

			float currentAverageAngle = AverageAngleInList(_points);


			Debug.WriteLine("currentAverageAngle: {0}", currentAverageAngle);

			if (_processedPoints.Count > 1)
			{

				var processedAverageAngle = AverageAngleInList(_processedPoints);
				Debug.WriteLine("processedAverageAngle: {0}", processedAverageAngle);

				var angleDiff = (currentAverageAngle - processedAverageAngle);
				angleDiff = SmallestAngle(angleDiff);

				var angleArray = AnglesInList(_points);

				var cutoffIndex = -1;
				foreach (var angle in angleArray)
				{
					var alternativeAngleDiff = SmallestAngle(Math.Abs(angle - processedAverageAngle));
					if ((alternativeAngleDiff) > (Math.Abs(angleDiff) * 3))
					{
						angleDiff = alternativeAngleDiff;
						cutoffIndex = angleArray.IndexOf(angle);
					}
				}

				Debug.WriteLine("angleDiff: {0}", angleDiff);
				if (Math.Abs(angleDiff) > Math.PI / 4f)
				{
					var lastPoint = _processedPoints.Last();
					ProcessPointForLine();

					if (cutoffIndex == -1)
					{
                        Debug.WriteLine("cutoffindex=-1");
						_processedPoints.AddRange(_points);
						_points.Clear();
					}
					else
					{
                        Debug.WriteLine($"cutoffindex={cutoffIndex}"); 
						_processedPoints.AddRange(_points.GetRange(0, cutoffIndex + 1));
						_points.RemoveRange(0, cutoffIndex + 1);
					}
				}
			}

			_processedPoints.AddRange(_points);
			_points.Clear();
		}

		public static float AngleDiff(float angleA, float angleB)
		{
			return 0;
		}

		public static float SmallestAngle(float angle)
		{
			while (angle < -(float)Math.PI)
			{
				angle += 2f * (float)Math.PI;
			}
			return (float)Math.Min((float)Math.PI * 2 - angle, angle);
		}

		public static float AverageAngleInList(List<Point> points)
		{
			if (points.Count < 2)
			{
				throw new Exception("You must have at least two points in the list");
			}

			var dx = points.Average(e => e.X);
			var dy = points.Average(e => e.Y);

			dx -= points.First().X;
			dy -= points.First().Y;

			float angle = (float)System.Math.Atan2(dy, dx);
			return SmallestAngle(angle);
		}

		/// <summary>
		/// Calculates the angle between each point
		/// </summary>
		/// <returns>The angles in list.</returns>
		/// <param name="points">Points.</param>
		public static List<float> AnglesInList(List<Point> points)
		{
			var list = new List<float>();

			if (points.Count < 2)
			{
				return list;
			}

			for (int index = 0; index < points.Count - 1; index++)
			{
				var dx = points[index + 1].X - points[index].X;
				var dy = points[index + 1].Y - points[index].Y;

				list.Add(SmallestAngle((float)System.Math.Atan2(dy, dx)));
			}

			return list;
		}

		private void ProcessPointForLine()
		{
			_lines.Add(new Line()
			{
				P1 = _processedPoints.First(),
				P2 = _processedPoints.Last()
			});
			_processedPoints.Clear();

			if (LineFound != null)
			{
				LineFound(_lines.Last());
			}
		}

		public List<Line> End()
		{
			if (_processedPoints.Count > 0)
			{
				_lines.Add(new Line() { P1 = _processedPoints.First(), P2 = _processedPoints.Last() });
			}

			_processedPoints.Clear();

			if (LineFound != null && _lines.Count > 0)
			{
				LineFound(_lines.Last());
			}

			if (RecognitionEnded != null)
			{
				RecognitionEnded(_lines);
			}

			return _lines;
		}
	}
}
