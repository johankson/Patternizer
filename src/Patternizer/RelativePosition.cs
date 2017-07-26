using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Patternizer
{

    /// <summary>
    /// This is a descriptive flag to allow for relative positioning 
    /// when tracking lines and movements.
    /// CONCEPT: IT IS NOT COMPLETE
    /// </summary>
    [Flags]
    public enum RelativePosition
    {
        Anywhere = 0,

        AboveTop = 1,
        NearTop = 2,
        BelowTop = 4,

        OnTheRightOf = 8,
        NearRightSide = 16,
        BeforeRightSide = 32,

		OnTheLeftOf = 64,
		NearLeftSide = 128,
		AfterLeftSize = 256,

		AboveBottom = 512,
		NearBottom = 1024,
		BelowBottom = 2048,

        NearStart = 4096
    }

	[Flags]
	public enum BoundsDescriptor
	{
		Undefined = 0,

		IsWide = 1,
		IsTall = 2
	}
}