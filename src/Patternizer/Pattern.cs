using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Patternizer
{

	public class Pattern : PatternBase 
	{
		private List<PatternBase> _children = new List<PatternBase>();

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

		public override PatternResult Evaluate(List<Line> lines, StepContext context)
		{
			PatternResult result = null;

			foreach (var item in _children) 
            {
				result = item.Evaluate (lines, context);
				if (!result.IsValid) {
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
