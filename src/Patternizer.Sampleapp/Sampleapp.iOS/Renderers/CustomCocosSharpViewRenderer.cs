using System;
using Renderers;
using Xamarin.Forms;
using CocosSharp;
using Xamarin.Forms.Platform.iOS;
using Sampleapp;

[assembly:ExportRenderer(typeof(MyCocosSharpView), typeof(CustomCocosSharpViewRenderer))]

namespace Renderers
{
	public class CustomCocosSharpViewRenderer : CocosSharp.CocosSharpViewRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<CocosSharpView> e)
		{
			base.OnElementChanged (e);

			if (Control != null) {
				Control.MultipleTouchEnabled = true;
			}
		}
	}
}