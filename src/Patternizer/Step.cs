using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace Patternizer
{
	internal abstract class Step
	{
        public RelativePosition RelativeStartConstraint { get; set; } = RelativePosition.Anywhere;

        public RelativePosition RelativeEndConstraint { get; set; } = RelativePosition.Anywhere;

		public BoundsDescriptor BoundsConstraint { get; set; } = BoundsDescriptor.Undefined;

        /// <summary>
        /// Evaluate the specified lines.
        /// </summary>s
        /// <remarks>Remember to remove the lines that you have evaluated</remarks>
        /// <param name="lastStepEndPoint">The last point of the previous process line or null if this is the first line</param> 
        /// <param name="lines">Lines.</param>
        public abstract StepPatternEvaluationResult Evaluate (Point? lastStepEndPoint, List<Line> lines, StepContext context);

        /// <summary>
        /// Removes the first line from the lines collection
        /// </summary>
        /// <returns>The line.</returns>
        /// <param name="lines">Lines.</param>
        /// <param name="context">Context.</param>
        public bool PopLine(List<Line> lines, StepContext context)
        {
            var line = lines.First();

            // Evaluate
            if(!EvaluateStartAndEndConstraints(line, context))
            {
                Debug.WriteLine("Start or End Constraint validation was negative");
                return false;
            }
            
			context.PushLine(line);
            lines.RemoveAt(0);

			// Evaluate the bounds right after the line is accepted
			return EvaluateBoundsConstraint(context);
        }

		private bool EvaluateStartAndEndConstraints(Line line, StepContext context)
		{
			if (RelativeStartConstraint == RelativePosition.Anywhere && RelativeEndConstraint == RelativePosition.Anywhere)
			{
				return true;
			}

			if (RelativeStartConstraint > 0)
			{
				if (!Evaluate(RelativeStartConstraint, line.P1, context))
					return false;
			}

			if (RelativeEndConstraint > 0)
			{
				if (!Evaluate(RelativeEndConstraint, line.P2, context))
					return false;
			}

			return true;
		}

		private bool EvaluateBoundsConstraint(StepContext context)
		{
			if (BoundsConstraint == 0)
			{
				return true;
			}

			return EvaluateBounds(BoundsConstraint, context);
		}

		private bool EvaluateBounds(BoundsDescriptor flags, StepContext context)
		{
			// Concept code, this should be checked for invalid combinations
			if (flags.HasFlag(BoundsDescriptor.IsWide) && context.Width < context.Height * Settings.WideCutoffValue)
			{
                Debug.WriteLine($"Evaluation failed: Bounds was set to IsWide but the shape isn't wide. Width={context.Width}, Height={context.Height}, WideCutoffValue value={Settings.WideCutoffValue}");
				return false;
			}

			if (flags.HasFlag(BoundsDescriptor.IsTall) && context.Height < context.Width * Settings.TallCutoffValue)
			{
                // TODO Add debug output
				return false;
			}

			return true;
 		}

		private bool Evaluate(RelativePosition flags, Point p, StepContext context)
		{
			if (flags.HasFlag(RelativePosition.AboveBottom))
				throw new NotImplementedException();
			if (flags.HasFlag(RelativePosition.AboveTop))
				throw new NotImplementedException();
			if (flags.HasFlag(RelativePosition.AfterLeftSize))
				throw new NotImplementedException();
			if (flags.HasFlag(RelativePosition.BeforeRightSide))
				throw new NotImplementedException();
			if (flags.HasFlag(RelativePosition.BelowBottom))
				throw new NotImplementedException();
			if (flags.HasFlag(RelativePosition.BelowTop))
				throw new NotImplementedException();
			if (flags.HasFlag(RelativePosition.OnTheLeftOf))
				throw new NotImplementedException();
			if (flags.HasFlag(RelativePosition.OnTheRightOf))
                throw new NotImplementedException();

			if (flags.HasFlag(RelativePosition.NearTop) && !IsNearTop(p, context))
			{
				return false;
			}

			if (flags.HasFlag(RelativePosition.NearBottom) && !IsNearBottom(p, context))
			{
				return false;
			}

			if (flags.HasFlag(RelativePosition.NearLeftSide) && !IsNearLeft(p, context))
			{
				return false;
			}

			if (flags.HasFlag(RelativePosition.NearRightSide) && !IsNearRight(p, context))
			{
				return false;
			}

            if( flags.HasFlag(RelativePosition.NearStart) && !IsNearStart(p, context))
            {
                return false;
            }

			return true;
		} 

        private bool IsNearTop(Point p, StepContext context)
		{
			return Math.Abs(context.Top - p.Y) <= (Settings.UnitValue * Settings.NearUnitValue);
		}

		private bool IsNearBottom(Point p, StepContext context)
		{
			return Math.Abs(context.Bottom - p.Y) <= (Settings.UnitValue * Settings.NearUnitValue);
		}

		private bool IsNearLeft(Point p, StepContext context)
		{
			return Math.Abs(context.Left - p.X) <= (Settings.UnitValue * Settings.NearUnitValue);
		}

		private bool IsNearRight(Point p, StepContext context)
		{
			return Math.Abs(context.Right - p.X) <= (Settings.UnitValue * Settings.NearUnitValue);
		}

        private bool IsNearStart(Point p, StepContext context)
        {
            if (!context.HistoricalLines.Any())
            {
                throw new Exception("There must be at least one line to determine if we are near start");
            }

            var firstPoint = context.HistoricalLines.First().P1;
            var delta = p - firstPoint;

            return delta.Magnitude <= (Settings.UnitValue * Settings.NearUnitValue);
        }
    }
}
