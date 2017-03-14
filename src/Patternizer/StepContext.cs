using System.Linq;
using System.Collections.Generic;
using System;

namespace Patternizer
{
    public class StepContext
    {
		public float Right { get; set; }
		public float Left { get; set; }
		public float Top { get; set; }
		public float Bottom { get; set; }

		public float Width
		{
			get
			{
				return Right - Left;
			}
		}

		public float Height
		{
			get
			{
				return Top - Bottom;
			}
		}

		public List<Line> HistoricalLines { get; private set; } = new List<Line>();

		public void PushLine(Line line)
		{
			HistoricalLines.Add(line);
			UpdateBounds();
		}

	 	void UpdateBounds()
		{
			Left = Math.Min(HistoricalLines.Min(e => e.P1.X), HistoricalLines.Min(e => e.P2.X));
			Right = Math.Max(HistoricalLines.Max(e => e.P1.X), HistoricalLines.Max(e => e.P2.X));

			Bottom = Math.Min(HistoricalLines.Min(e => e.P1.Y), HistoricalLines.Min(e => e.P2.Y));
			Top = Math.Max(HistoricalLines.Max(e => e.P1.Y), HistoricalLines.Max(e => e.P2.Y));
		}
	}
}
