using System;

namespace Patternizer
{
	public static class Settings
	{
		/// <summary>
		/// The lowest movement to be tracked as a unit
		/// </summary>
		/// <value>The unit value.</value>
		public static float UnitValue { get; set; } = 30f;

		/// <summary>
		/// The slope value is how much a line can differ from
		/// being though of as straight. Used in X or Y aligned line
		/// recognition.
		/// </summary>
		/// <value>The slope ratio.</value>
		/// <remarks>
		/// If vertical the line is 4 units long (for example 4*30 = 120 points) and
		/// the SlopeRatio is 0.2, then the x can differ 4*30*0.3 points = 36 points.
		/// </remarks>
		public static float AcceptedSlopeRatio { get; set; } = 0.3f;

		/// <summary>
		/// Determines the amount of units that counts as a near evaluation
		/// </summary>
		/// <value>The near unit value.</value>
		public static float NearUnitValue { get; set; } = 4f;

		/// <summary>
		/// Determines the ratio of how much wider a shape has to be in contrast
		/// to the height of the shape.
		/// </summary>
		/// <remarks>
		/// For example, if the value is set to 2 and the height of the shape
		/// is 30, then the width has to be at least 60 (30*2) in order to be considered
		/// wide.
		/// </remarks>
		/// <value>The wide cutoff value.</value>
		public static float WideCutoffValue { get; set; } = 2f;

		/// <summary>
		/// Determines the ratio of how much taller a shape has to be in contrast
		/// to the width of the shape.
		/// </summary>
		/// <remarks>
		/// For example, if the value is set to 2 and the width of the shape
		/// is 30, then the height has to be at least 60 (30*2) in order to be considered
		/// tall.
		/// </remarks>
		/// <value>The wide cutoff value.</value>
		public static float TallCutoffValue { get; set; } = 2f;

		/// <summary>
		/// The number of times the list of lines should be simplified when
		/// evaluating patterns.
		/// </summary>
		/// <value>The line simplification pass count.</value>
		public static int LineSimplificationPassCount { get; set; } = 3;

		/// <summary>
		/// The amount to increase the cut off value for each 
		/// line simplification step.
		/// </summary>
		/// <value>The line simplification step value.</value>
		public static float LineSimplificationStepValue { get; set; } = 20f;
	}
}