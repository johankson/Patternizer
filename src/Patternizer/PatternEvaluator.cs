using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Diagnostics;

namespace Patternizer
{
	public class PatternEvaluator
	{
		private Dictionary<string, Pattern> _patterns = new Dictionary<string, Pattern>();

		public Pattern Add(string key)
		{
			if (_patterns.ContainsKey (key)) {
				throw new ArgumentException ("Key already exists in collection");
			}

			var pattern = new Pattern ();
			_patterns.Add (key, pattern);
			return pattern;
		}

		public PatternEvaluationResult Evaluate(List<Line> lines)
		{
			Debug.WriteLine("EVALUATION START");

			var pass = 1;
			while (pass <= Settings.LineSimplificationPassCount)
			{
				Debug.WriteLine($"Pass {pass}");
				foreach (var line in lines)
				{
					Debug.WriteLine($"Line {(int)line.P1.X}, {(int)line.P1.Y} -> {(int)line.P2.X}, {(int)line.P2.Y}");
				}

				var context = new StepContext();

				foreach (var item in _patterns)
				{
					var linesToEvaluate = lines.ToList();
					var pattern = item.Value;

					var result = pattern.Evaluate(linesToEvaluate, context);
					if (result.IsValid && linesToEvaluate.Count == 0)
					{
						return new PatternEvaluationResult()
						{
							IsValid = true,
							Reason = PatternEvaluationReasons.PatternMatch,
							Pattern = item.Value,
							Key = item.Key,
							UpperLeft = new Point()
							{
								X = Math.Min(lines.Min(e => e.P1.X), lines.Min(e => e.P2.X)),
								Y = Math.Max(lines.Min(e => e.P1.Y), lines.Min(e => e.P2.Y))
							},
							LowerRight = new Point()
							{
								X = Math.Max(lines.Max(e => e.P1.X), lines.Max(e => e.P2.X)),
								Y = Math.Min(lines.Max(e => e.P1.Y), lines.Max(e => e.P2.Y))
							}

						};
					}
				}

				if (pass <= Settings.LineSimplificationPassCount)
				{
					// Simplify lines by removing short lines
					foreach (var line in lines.ToList())
					{
						if (line.Length < (pass * Settings.LineSimplificationStepValue))
						{
							Debug.WriteLine($"Removing line {line}");
							lines.Remove(line);
						}
					}
				}

				pass++;
			}

			return new PatternEvaluationResult () 
			{ 
				IsValid = false,
				Reason = PatternEvaluationReasons.PatternMismatch,
				Pattern = null,
				Key = null
			};
		}
	}
}