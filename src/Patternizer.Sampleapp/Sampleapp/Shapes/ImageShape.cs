using System;
using CocosSharp;

namespace Sampleapp.Shapes
{
	public class ImageShape : RectangleShape
	{
		public override void Draw (CocosSharp.CCDrawNode node)
		{
			node.DrawRect(Bounds(), CCColor4B.Transparent, LINE_THICKNESS, CCColor4B.Green);
		}
	}
}