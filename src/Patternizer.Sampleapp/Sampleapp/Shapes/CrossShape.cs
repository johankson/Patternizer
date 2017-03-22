using System;
using System.Collections.Generic;
using CocosSharp;
using System.Linq;
using Patternizer;

namespace Sampleapp.Shapes
{

	public class CrossShape : Shape
	{
		public Line L1 {
			get;
			set;
		}
		public Line L2 {
				get;
			set;
		}

		public override void Draw (CCDrawNode node)
		{
			node.DrawLine (L1.P1.ToCCPoint(), L1.P2.ToCCPoint(), LINE_THICKNESS, CCColor4B.Red, CCLineCap.Round);
			node.DrawLine (L2.P1.ToCCPoint(), L2.P2.ToCCPoint(), LINE_THICKNESS, CCColor4B.Red, CCLineCap.Round);
		}

		public override CCRect Bounds ()
		{
			return CalculateBounds (L1.P1.ToCCPoint(), L1.P2.ToCCPoint(), L2.P1.ToCCPoint(), L2.P2.ToCCPoint());
		}
	}
}