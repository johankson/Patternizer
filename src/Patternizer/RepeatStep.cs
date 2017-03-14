using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Patternizer
{

	internal class RepeatStep : Step
	{
		private int _repeatCount;
		private Expression<Func<StepPattern, StepPattern>> _pattern;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatStep"/> class.
        /// </summary>
        /// <param name="repeatCount">The repeat count, if > 0 then it must be an exact match. Otherwise 1-* matches is ok</param>
        /// <param name="pattern">Pattern.</param>
        public RepeatStep (int repeatCount, Expression<Func<StepPattern, StepPattern>> pattern)
		{
			_repeatCount = repeatCount;
			_pattern = pattern;
		}

		public override StepPatternEvaluationResult Evaluate (Point? lastStepEndPoint, List<Line> lines, StepContext context)
		{
			if (_repeatCount > 0) {
				throw new NotImplementedException ("Repeat count is not implemented yet");
			}

			var preEvaluationList = new List<Line> (lines);

			var func = _pattern.Compile ();
			var isValid = false;
			PatternResult lastResult = null;
			var lastValidCount = preEvaluationList.Count;

			bool control = true;
			while (control) {
				if (preEvaluationList.Count == 0) {
					control = false;
					continue;
				}

				var pattern = func.Invoke (new StepPattern());
				var result = pattern.Evaluate (preEvaluationList, context);
				if (result.IsValid) {
					isValid = true;
					lastValidCount = preEvaluationList.Count;
				} else {
					control = false;
				}

				lastResult = result;
			}

			if (isValid) {
				lines.RemoveRange(0, lines.Count - lastValidCount);
			}

			return new StepPatternEvaluationResult (isValid, lastResult.LastPointInPattern);
		}
	}
}