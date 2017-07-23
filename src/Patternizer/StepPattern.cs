using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Patternizer
{
	/// <summary>
	/// Descripes a step by step pattern
	/// </summary>
	public class StepPattern : PatternBase 
	{
		private List<Step> _steps = new List<Step>();

		public StepPattern MovesRight()
		{
			_steps.Add (new MovesRightStep ());
			return this;
		}

		public StepPattern MovesUp()
		{
			_steps.Add (new MovesUpStep ());
			return this;
		}

		public StepPattern MovesDown()
		{
			_steps.Add (new MovesDownStep());
			return this;
		}

		public StepPattern MovesLeft()
		{
			_steps.Add (new MovesLeftStep());
			return this;
		}

		public StepPattern MovesLeftAndDown()
		{
			_steps.Add (new MovesLeftAndDownStep());
			return this;
		}

        public StepPattern MovesLeftAndUp()
        {
            _steps.Add(new MovesLeftAndUpStep());
            return this;
        }

		public StepPattern MovesRightAndDown()
		{
			_steps.Add (new MovesRightAndDownStep());
			return this;
		}

		public StepPattern MovesRightAndUp()
		{
			_steps.Add(new MovesRightAndUpStep());
			return this;
		}

		public StepPattern Repetitive(int repeatCount, Expression<Func<StepPattern, StepPattern>> p)
		{
			_steps.Add(new RepeatStep(repeatCount, p));
			return this;
		}

		public StepPattern Repetitive(Expression<Func<StepPattern, StepPattern>> p)
		{
			return Repetitive (0, p);
		}

        #region Constraints

        /// <summary>
        /// Adds a start constraint to the last step registered.
        /// </summary>
        /// <param name="relativeStartPosition">Relative start position.</param>
        public StepPattern Start(RelativePosition relativeStartPosition)
        {
            if (_steps.Count == 0)
            {
                throw new Exception("The has to be at least one step in the pattern");
            }

            var lastStep = _steps.Last();
            lastStep.RelativeStartConstraint = relativeStartPosition;

            return this;
        }

        /// <summary>
        /// Adds a start constraint to the last step registered.
        /// </summary>
        /// <param name="relativeStartPosition">Relative start position.</param>
        public StepPattern End(RelativePosition relativeStartPosition)
        {
            if (_steps.Count == 0)
            {
                throw new Exception("The has to be at least one step in the pattern");
            }

            var lastStep = _steps.Last();
            lastStep.RelativeEndConstraint = relativeStartPosition;

            return this;
        }

		public StepPattern Bounds(BoundsDescriptor bounds)
		{
			if (_steps.Count == 0)
			{
				throw new Exception("The has to be at least one step in the pattern");
			}

			var lastStep = _steps.Last();
			lastStep.BoundsConstraint = bounds;

			return this;
		}

        #endregion

		public override PatternResult Evaluate(List<Line> lines, StepContext context)
		{
			if (_steps.Count == 0) 
            {
				throw new Exception ("There are no steps in this pattern?");
			}

			StepPatternEvaluationResult lastResult = null;

			foreach (var step in _steps)
            {
				var stepResult = step.Evaluate(lastResult?.LastPoint, lines, context);
				if (stepResult.IsValid == false)
                {
					return new PatternResult () { IsValid = false, LastPointInPattern = stepResult.LastPoint };
				}

				lastResult = stepResult;
			}

			return new PatternResult () { IsValid = true, LastPointInPattern = lastResult.LastPoint };
		}
	}
}
