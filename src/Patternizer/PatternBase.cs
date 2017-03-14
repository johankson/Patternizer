using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Patternizer
{
	public abstract class PatternBase
	{
		public abstract PatternResult Evaluate (List<Line> lines, StepContext context);
	}
}