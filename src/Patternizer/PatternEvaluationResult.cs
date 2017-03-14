using System;
using System.Collections.Generic;

namespace Patternizer
{
	public class PatternEvaluationResult
	{
		public bool IsValid { get; set; }
		public PatternEvaluationReasons Reason { get; set; }
		public Pattern Pattern { get; set; }
		public string Key { get; set; }

		public Point UpperLeft { get; set; }
		public Point LowerRight { get; set; }
	}
}