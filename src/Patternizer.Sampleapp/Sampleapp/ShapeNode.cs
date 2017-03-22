using System;
using CocosSharp;
using System.Diagnostics;
using Shapes;
using Sampleapp.Shapes;

namespace Sampleapp
{
	public class ShapeNode : CCDrawNode
	{
		private RectangleShape _shape;

		public ShapeNode (RectangleShape shape)
		{
			_shape = shape;
			UpdateGui ();
		}

		public virtual void EvaulateTap(CCPoint point)
		{
		}

		public CCPoint P1 {
			get { return _shape.P1; }
			set {
				_shape.P1 = value;
				UpdateGui ();
			}
		}

		public CCPoint P2 {
			get { return _shape.P2; }
			set {
				_shape.P2 = value;

				UpdateGui ();
			}
		}

		public void SetNewPoints(CCPoint p1, CCPoint p2)
		{
			P1 = p1;
			P2 = p2;
		}

		private void UpdateGui()
		{
			Clear ();
			Position = _shape.P1;
			ContentSize = new CCSize (_shape.P2.X - _shape.P1.X, _shape.P2.Y - _shape.P1.Y);

			var rect = new CCRect (0, 0, this.ContentSize.Width, this.ContentSize.Height);

			// TODO let the shapes draw them selves? Renderers?
			if (_shape is ImageShape) {
				this.DrawRect (rect, CCColor4B.White, 6, CCColor4B.Gray);
				this.DrawLine (new CCPoint(0, 0), rect.UpperRight, 6, CCColor4B.Gray);
				this.DrawLine (new CCPoint (0, rect.MaxY), new CCPoint (rect.MaxX, 0), 6, CCColor4B.Gray);

				return;
			} 
			if (_shape is EntryShape) {
				this.DrawLine (new CCPoint(0, 40), new CCPoint(0,0), 6, CCColor4B.Gray, CCLineCap.Round);
				this.DrawLine (new CCPoint(0, 0), new CCPoint(rect.MaxX,0), 6, CCColor4B.Gray, CCLineCap.Round);
				this.DrawLine (new CCPoint(rect.MaxX, 40), new CCPoint(rect.MaxX,0), 6, CCColor4B.Gray, CCLineCap.Round);

				return;
			} 
			else if (_shape is TextShape) {
				this.DrawLine (new CCPoint(0, 0), new CCPoint(rect.MaxX, 0), 6, CCColor4B.LightGray);

				for (int y = 0; y < rect.MaxY; y += 40) {
					this.DrawLine (new CCPoint (0, y), new CCPoint (rect.MaxX, y), 6, CCColor4B.LightGray);
				}

				this.DrawLine (new CCPoint (0, rect.MaxY), new CCPoint (rect.MaxX, rect.MaxY), 6, CCColor4B.LightGray);

				return;
			} 
			else if (_shape is LineOfTextShape)
			{
				if (this.ChildrenCount == 0)
				{
					var text = new CCLabel("Lorem ipsum", "Arial", 60, CCLabelFormat.SystemFont);
					text.Position = new CCPoint(this.ContentSize.Width/2f, 0);
					text.Color = CCColor3B.Gray;
					this.AddChild(text);
				}

				return;
			}
			else if (_shape is RectangleShape) {
				this.DrawRect (rect, CCColor4B.White, 6, CCColor4B.Gray);
				return;
			}
		}
	}
}