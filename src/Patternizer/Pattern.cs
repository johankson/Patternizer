using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Patternizer
{
	public class Pattern : PatternBase 
	{
		private List<PatternBase> _children = new List<PatternBase>();

        // Context related
        private StepContext _originalStepContext;
        private List<Line> _originalLines;

        // Inverse related
        private bool _allowInverse;
        private InverseDescriptor _invertDescriptor;

        // Any order related
        private bool _allowAnyOrder;
        private int _allowAnyOrderStepsLeft;

        public Pattern When(PatternBase pattern)
		{
			if (pattern == null) {
				throw new ArgumentNullException ();
			}

			_children.Add (pattern);
			return this;
		}

		public Pattern When(Expression<Func<StepPattern, StepPattern>> p)
		{
			var imp = p.Compile ();
			var pattern = imp.Invoke (new StepPattern ());
			_children.Add (pattern);
			return this;
		}

        public Pattern AllowAnyOrder()
        {
            _allowAnyOrder = true;
            return this;
        }

        public Pattern AllowInverse(InverseDescriptor descriptor = InverseDescriptor.Horizontal)
        {
            _allowInverse = true;
            _invertDescriptor = descriptor;
            return this;
        }
     
        public override PatternResult Evaluate(List<Line> lines, StepContext context)
		{
			PatternResult result = null;

            if(_allowInverse)
            {
                StoreContext(context, lines);
            }

            if(_allowAnyOrder)
            {
                _allowAnyOrderStepsLeft = lines.Count - 1;
                StoreContext(context, lines);
            }

            foreach (var item in _children) 
            {
				result = item.Evaluate (lines, context);
				if (!result.IsValid)
                {
                    if(_allowInverse)
                    {
                        // REMARK: This is just concept code and subject to change

                        // restore the context
                        RestoreOriginalContext(context, lines);

                        // Crude way to flip the lines
                        InverseLines(lines);

                        // do the inverse 
                        _allowInverse = false;
                        return Evaluate(lines, context);
                    }

                    if(_allowAnyOrder)
                    {
                        // REMARK: This is just concept code and subject to change.
                        // This implementation has the following issues:
                        //
                        // 1)   It looks ugly
                        // 2)   It will reorder lines that are possibly outside the scope of this
                        //      pattern. That can be calculated (not easily for Repeat-patterns though)
                        //      at best. Perhaps through a Pattern.NumberOfLinesNeeded() property?

                        while(_allowAnyOrderStepsLeft > 0)
                        {
                            // Restore the context
                            RestoreOriginalContext(context, lines);

                            // ReorderLines
                            ReorderLines(lines);
                            _originalLines = lines.ToList();

                            // Evaluate
                            result = item.Evaluate(lines, context);
                            if(result.IsValid)
                            {
                                return result;
                            }

                            _allowAnyOrderStepsLeft--;
                        }
                    }

                    return result;
				}
			}

			return result;
		}

        private void ReorderLines(List<Line> lines)
        {
            var first = lines.First();
            lines.Remove(first);
            lines.Add(first);
        }

        private void InverseLines(List<Line> lines)
        {
            var copy = lines.ToList();
            lines.Clear();
            var x = _invertDescriptor == InverseDescriptor.Horizontal || _invertDescriptor == InverseDescriptor.Both ? -1 : 1;
            var y = _invertDescriptor == InverseDescriptor.Vertical || _invertDescriptor == InverseDescriptor.Both ? -1 : 1;

            foreach (var line in copy)
            {
                lines.Add(
                    new Line(
                        new Point(line.P1.X * x, line.P1.Y * y),
                        new Point(line.P2.X * x, line.P2.Y * y)));
            }
        }

        private void StoreContext(StepContext context, List<Line> lines)
        {
            _originalStepContext = new StepContext()
            {
                Bottom = context.Bottom,
                Left = context.Left,
                Top = context.Top,
                Right = context.Right,
                HistoricalLines = context.HistoricalLines.ToList()
            };

            _originalLines = lines.ToList();
        }

        private void RestoreOriginalContext(StepContext context, List<Line> lines)
        {
            lines.Clear();
            lines.AddRange(_originalLines);
            context.Left = _originalStepContext.Left;
            context.Right = _originalStepContext.Right;
            context.Top = _originalStepContext.Top;
            context.Bottom = _originalStepContext.Bottom;
            context.HistoricalLines = _originalStepContext.HistoricalLines;
        }

        #region Predefined Patterns

        public static PatternBase WideRectangle
		{
			get 
            { 
				var pattern = new StepPattern ();
				pattern.MovesRight()
					   .MovesDown()
					   .MovesLeft()
					   .MovesUp().End(RelativePosition.NearTop | RelativePosition.NearLeftSide)
					   .Bounds(BoundsDescriptor.IsWide);
				return pattern;
			}
		}

        public static PatternBase Cross
        {
            get
            {
                var pattern = new StepPattern();

                pattern.MovesRightAndDown();
                pattern.MovesLeftAndDown()
                       .Start(RelativePosition.NearTop | RelativePosition.NearRightSide)
                       .End(RelativePosition.NearBottom | RelativePosition.NearLeftSide);

                return pattern;
            }
        }

        #endregion
    }
}
