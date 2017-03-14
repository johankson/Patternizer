using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Patternizer
{

	internal class StepPatternEvaluationResult
	{
		public StepPatternEvaluationResult (bool isValid, Point? lastPoint)
		{
			IsValid = isValid;
			LastPoint = lastPoint;
		}

		public bool IsValid { get; set; }
		public Point? LastPoint { get; set; }
	}

}