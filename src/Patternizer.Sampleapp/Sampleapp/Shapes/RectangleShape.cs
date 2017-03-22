using System;
using System.Collections.Generic;
using CocosSharp;
using System.Linq;

namespace Sampleapp.Shapes
{
	public class RectangleShape : Shape
	{
		public CCPoint P1 {
			get;
			set;
		}

		public CCPoint P2 {
			get;
			set;
		}

		public override void Draw (CCDrawNode node)
		{
			node.DrawRect(Bounds(), CCColor4B.Transparent, LINE_THICKNESS, CCColor4B.Green);
		}

		public override CCRect Bounds ()
		{
			return new CCRect (P1.X, P1.Y, P2.X - P1.X, P2.Y - P1.Y);
		}
	}
}