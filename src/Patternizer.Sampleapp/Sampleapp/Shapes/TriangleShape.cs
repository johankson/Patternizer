using System;
using System.Collections.Generic;
using CocosSharp;
using System.Linq;

namespace Sampleapp.Shapes
{
	public class TriangleShape : Shape
	{
		public CCPoint P1 {
			get;
			set;
		}

		public CCPoint P2 {
			get;
			set;
		}

		public CCPoint P3 {
			get;
			set;
		}

		public override void Draw (CCDrawNode node)
		{
			node.DrawLine (P1, P2, LINE_THICKNESS, CCColor4B.Red, CCLineCap.Round);
			node.DrawLine (P2, P3, LINE_THICKNESS, CCColor4B.Red, CCLineCap.Round);
			node.DrawLine (P3, P1, LINE_THICKNESS, CCColor4B.Red, CCLineCap.Round);
		}

		public override CCRect Bounds ()
		{
			return CalculateBounds (new List<CCPoint>() { P1, P2, P3 });
		}
	}
}