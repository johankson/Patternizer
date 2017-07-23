using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Patternizer
{

	public class Pattern : PatternBase 
	{
		private List<PatternBase> _children = new List<PatternBase>();
        private bool _allowInverse;
        private StepContext _originalStepContext;
        private List<Line> _originalLines;
        private InverseDescriptor _invertDescriptor;

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

        public Pattern AllowInverse(InverseDescriptor descriptor = InverseDescriptor.Horizontal)
        {
            _allowInverse = true;
            return this;
        }
     
        public override PatternResult Evaluate(List<Line> lines, StepContext context)
		{
			PatternResult result = null;

            if(_allowInverse)
            {
                // TODO: Better serialization/deserialization
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

			foreach (var item in _children) 
            {
				result = item.Evaluate (lines, context);
				if (!result.IsValid)
                {
                    if(_allowInverse)
                    {
                        // restore the context
                        // TODO: Better serialization/deserialization
                        lines.Clear();
                        context.Left = _originalStepContext.Left;
                        context.Right = _originalStepContext.Right;
                        context.Top = _originalStepContext.Top;
                        context.Bottom = _originalStepContext.Bottom;
                        context.HistoricalLines = _originalStepContext.HistoricalLines;

                        // Crude way to flip the lines horizontally
                        var x = _invertDescriptor == InverseDescriptor.Horizontal || _invertDescriptor == InverseDescriptor.Both ? -1 : 1;
                        var y = _invertDescriptor == InverseDescriptor.Vertical || _invertDescriptor == InverseDescriptor.Both ? -1 : 1;

                        foreach (var line in _originalLines)
                        {
                            lines.Add(
                                new Line(
                                    new Point(line.P1.X * x, line.P1.Y * y),
                                    new Point(line.P2.X * x, line.P2.Y * y)));
                        }

                        // do the inverse 
                        _allowInverse = false;
                        return Evaluate(lines, context);
                    }
					return result;
				}
			}

			return result;
		}

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
	}
}
