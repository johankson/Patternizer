using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Patternizer
{
	internal class MovesLeftStep : Step
	{
		public override StepPatternEvaluationResult Evaluate (Point? lastStepEndPoint, List<Line> lines, StepContext context)
		{
			if (lines.Count == 0) {
				return new StepPatternEvaluationResult(false, null);
			}

			var line = lines.First ();

			var dx = line.P2.X - line.P1.X;
			var dy = line.P2.Y - line.P1.Y;

			var acceptedSlopeDiff = Math.Abs(dx * Settings.AcceptedSlopeRatio);

			if (dx < Settings.UnitValue && Math.Abs (dy) < acceptedSlopeDiff && PopLine(lines, context)) 
            {
				return new StepPatternEvaluationResult (true, line.P2);
			}

			return new StepPatternEvaluationResult (false, null);
		}
	}
}