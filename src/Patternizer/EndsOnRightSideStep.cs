using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Patternizer
{

	class EndsOnRightSideStep : Step
	{
		public override StepPatternEvaluationResult Evaluate (Point? lastStepEndPoint, List<Line> lines, StepContext context)
		{
			if (lastStepEndPoint == null) {
				return new StepPatternEvaluationResult (false, null);
			}

			if (lines.Count == 0) {
				return new StepPatternEvaluationResult (false, null);
			}

			if (lastStepEndPoint.Value.X > 600) { // TODO fix magic number
				return new StepPatternEvaluationResult(true, lastStepEndPoint);
			}

			return new StepPatternEvaluationResult (false, null);
		}
	}

}