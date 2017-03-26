using System;
using System.Collections.Generic;
using CocosSharp;
using System.Diagnostics;
using System.Linq;
using Shapes;
using System.Threading.Tasks;
using Patternizer;
using Sampleapp.Shapes;

namespace Sampleapp
{
	public class MainLayer : CCLayerColor
	{
		private bool _drawDebug = true;

		private CCDrawNode _canvasNode;
		private CCDrawNode _shapeNode;
		private CCPoint _lastKnownPoint;
		private LineRecognizer _lineRecognizer;
		private ShapeNode _trackedShape;
		private CCDrawNode _recognizerCanvasNode;
		private List<Line> _buffer = new List<Line>();
		private const float _clearTTLResetValue = 1f;
		private float _clearTTL = 0f;

		private PatternEvaluator _patternEvaluator;

		public MainLayer() : base(CCColor4B.AliceBlue)
		{
			_canvasNode = new CCDrawNode();
			_recognizerCanvasNode = new CCDrawNode();
			_lineRecognizer = new LineRecognizer();
			_shapeNode = new CCDrawNode();

			AddChild(_shapeNode);
			AddChild(_canvasNode);
			AddChild(_recognizerCanvasNode);

			if (_drawDebug)
			{
				_lineRecognizer.LineFound = (line) => _recognizerCanvasNode.DrawLine(line.P1.ToCCPoint(), line.P2.ToCCPoint(), 5, CCColor4B.Blue);
			}

			// Register patterns (concept)
			_patternEvaluator = new PatternEvaluator();
			_patternEvaluator.Add("button").When(Pattern.WideRectangle);
            _patternEvaluator.Add("image").When(Pattern.Cross);
			_patternEvaluator.Add("text").When(p => p.Repetitive(ip => ip.MovesRight().MovesLeftAndDown()).MovesRight());
			_patternEvaluator.Add("entry").When(p => p.MovesDown().MovesRight().MovesUp().Bounds(BoundsDescriptor.IsWide));
			_patternEvaluator.Add("lineoftext").When(p => p.MovesRight());
			_patternEvaluator.Add("delete").When(p => p.MovesRightAndUp());
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene();

			// Use the bounds to layout the positioning of our drawable assets
			CCRect bounds = VisibleBoundsWorldspace;

			var l = new CCEventListenerTouchAllAtOnce();
			l.OnTouchesBegan += OnTouchesBegan;
			l.OnTouchesMoved += OnTouchesMoved;
			l.OnTouchesEnded += OnTouchesEnded;
			AddEventListener(l, this);

		}

		void OnTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
		{
			touches = FindUniqueTouches(touches);

			if (touches.Count == 1)
			{
				var touch = touches[0];

				_lastKnownPoint = touch.Location;

				_trackedShape = null;
				_lineRecognizer.Clear();
			}
			else if (touches.Count > 1)
			{

				EvaluateResizeOrMoveStart(touches);
			}
		}

		void EvaluateResizeOrMoveStart(List<CCTouch> touches)
		{
			touches = FindUniqueTouches (touches);

			var touch = touches [0];

			_lastKnownPoint = touch.Location;
			if (_shapeNode.Children != null) {
				foreach (var obj in _shapeNode.Children) {
					if (obj is ShapeNode) {
						if (obj.BoundingBox.ContainsPoint (_lastKnownPoint)) {
							_trackedShape = obj as ShapeNode;
						}
					}
				}
			}
		}

		void OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
		{
			touches = FindUniqueTouches(touches);
			_clearTTL = _clearTTLResetValue;

			if (touches.Count == 1)
			{
				var touch = touches[0];

				if (_trackedShape != null)
				{
					_trackedShape.Position += touch.Delta;
				}
				else
				{
					var location = touch.LocationOnScreen;
					_canvasNode.DrawLine(_lastKnownPoint, touch.Location, 5, CCColor4B.Red, CCLineCap.Round);

					if (_drawDebug)
					{
						_canvasNode.DrawSolidCircle(_lastKnownPoint, 5, CCColor4B.Green);
					}

					_lastKnownPoint = touch.Location;
					_lineRecognizer.RegisterPoint(_lastKnownPoint.X, _lastKnownPoint.Y);
				}
			}
			else if (touches.Count > 1 && _trackedShape != null)
			{
				var touch = touches[0];
				_trackedShape.Position += touch.Delta;

				var t1 = touches[0];
				var t2 = touches[1];

				CCPoint p1;
				CCPoint p2;

				if (t1.Location.X < t2.Location.X)
				{
					p1.X = t1.Delta.X;
					p2.X = t2.Delta.X;
				}
				else
				{
					p1.X = t2.Delta.X;
					p2.X = t1.Delta.X;
				}

				if (t1.Location.Y < t2.Location.Y)
				{
					p1.Y = t1.Delta.Y;
					p2.Y = t2.Delta.Y;
				}
				else
				{
					p1.Y = t2.Delta.Y;
					p2.Y = t1.Delta.Y;
				}

				_trackedShape.SetNewPoints(_trackedShape.P1 + p1, _trackedShape.P2 + p2);
			}
			else
			{
				_lineRecognizer.Clear();

				EvaluateResizeOrMoveStart(touches);
			}
		}

		private void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
		{
			touches = FindUniqueTouches(touches);

			if (_trackedShape != null)
			{
				_trackedShape = null;
				return;
			}

			var lines = _lineRecognizer.End();

			// Concept
			if (lines.Count == 0 && _shapeNode.ChildrenCount > 0)
			{
				
				// Most likely a touch, evaulate the touch on known child nodes
				foreach (var node in _shapeNode.Children.OfType<ShapeNode>())
				{
					node.EvaulateTap(touches.First().Location);
				}
			}
			else if (lines.Count > 0)
			{
				EvaluateLines(lines);
				Debug.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(_lineRecognizer.AllPoints));
			}
		}

		public override void Update (float dt)
		{
			_clearTTL -= dt;

			if (_clearTTL > 0 && _clearTTL < 0.3f)
			{
				IsColorModifiedByOpacity = true;
				IsOpacityCascaded = true;
				var diff = (_clearTTL / 0.3f) * 255;
				_recognizerCanvasNode.IsColorModifiedByOpacity = true;
				_recognizerCanvasNode.Opacity = (byte)(int)diff; 
			}

			if (_clearTTL < 0)
			{
				_buffer.Clear();
				_recognizerCanvasNode.Clear();
				_canvasNode.Clear();
			}

			base.Update(dt);
		}

		/// <summary>
		/// Evaluates the lines we have in the buffer
		/// </summary>
		/// <param name="lines">Lines</param>
		/// <remarks>
		/// We keep a buffer of lines. If a list of lines cannot be
		/// evaluated we wait for another second or so to allow for more lines.
		/// When that times out and no shape can be determined, then 
		/// we clear the buffer.
		/// </remarks>
		private void EvaluateLines(List<Line> lines)
		{
			_clearTTL = _clearTTLResetValue;
			_buffer.AddRange(lines);

			var result = _patternEvaluator.Evaluate(_buffer);
			if (!result.IsValid)
			{
				return;
			}

			Debug.WriteLine("evaluation succeeded: " + result.Key);

			switch (result.Key)
			{
				case "button":
					var rectShape = new RectangleShape()
					{
						P1 = result.UpperLeft.ToCCPoint(),
						P2 = result.LowerRight.ToCCPoint()
					};
					_shapeNode.AddChild(new ShapeNode(rectShape));
					_recognizerCanvasNode.Clear();
					break;
				case "image":
					var imageShape = new ImageShape()
					{
						P1 = result.UpperLeft.ToCCPoint(),
						P2 = result.LowerRight.ToCCPoint()
					};
					_shapeNode.AddChild(new ShapeNode(imageShape));
					_recognizerCanvasNode.Clear();
					break;
				case "text":
					var textShape = new TextShape()
					{
						P1 = result.UpperLeft.ToCCPoint(),
						P2 = result.LowerRight.ToCCPoint()
					};
					_shapeNode.AddChild(new ShapeNode(textShape));
					_recognizerCanvasNode.Clear();
					break;
				case "entry":
					var entryShape = new EntryShape()
					{
						P1 = result.UpperLeft.ToCCPoint(),
						P2 = result.LowerRight.ToCCPoint()
					};
					_shapeNode.AddChild(new ShapeNode(entryShape));
					_recognizerCanvasNode.Clear();
					break;

				case "lineoftext":

					var lineoftext = new LineOfTextShape()
					{
						P1 = result.UpperLeft.ToCCPoint(),
						P2 = result.LowerRight.ToCCPoint()
					};
					_shapeNode.AddChild(new ShapeNode(lineoftext));
					_recognizerCanvasNode.Clear();
					break;

				
				case "delete":
					var deleteShape = new DeleteShape()
					{
						P1 = result.UpperLeft.ToCCPoint(),
						P2 = result.LowerRight.ToCCPoint()
					};

					var victims = IsShapeOverOtherShapes(deleteShape);
					foreach (var victim in victims)
					{
						_shapeNode.RemoveChild(victim);
					}
					_recognizerCanvasNode.Clear();
					break;

			}

			_canvasNode.Clear();
			_buffer.Clear();
		}

		private ShapeNode IsShapeWithinOtherShape(Shape shape)
		{
			if (_shapeNode.ChildrenCount == 0)
			{
				return null;
			}

			foreach (var item in _shapeNode.Children.OfType<ShapeNode>())
			{
				var bounds = shape.Bounds();
				if (item.BoundingBox.MinX < bounds.MinX &&
				    item.BoundingBox.MaxX > bounds.MaxX &&
				    item.BoundingBox.MinY < bounds.MinY &&
				    item.BoundingBox.MaxY > bounds.MaxY)
				{
					return item;
				}
			}

			return null;
		}

		private List<ShapeNode> IsShapeOverOtherShapes(Shape shape)
		{
			if (_shapeNode.ChildrenCount == 0)
			{
				return null;
			}

			var result = new List<ShapeNode>();

			foreach (var item in _shapeNode.Children.OfType<ShapeNode>())
			{
				var bounds = shape.Bounds();
				if (bounds.IntersectsRect(item.BoundingBox))
					result.Add(item);
			}

			return result;
		}

		private static List<CCTouch> FindUniqueTouches (List<CCTouch> touches)
		{
			var list = new List<CCTouch>();
			foreach (var touch in touches)
			{
				if (!list.Any(e => e.Id == touch.Id))
				{
					list.Add(touch);
				}
			}
			return list;
		}
	}
}
