/* using System;
using LineRecognizerLib;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Patternizer
{
	public class LineParser
	{
		private List<Line> _lines;

		public LineParser (List<Line> lines)
		{
			_lines = new List<Line> (lines);
		}

		public PatternResult Execute(Expression<Func<StepPattern, StepPattern>> p)
		{
			var imp = p.Compile ();
			var pattern = imp.Invoke (new StepPattern ());

			var result = pattern.Evaluate(_lines);
			return result;
		} 
	}
}
*/