using System;
using System.Collections.Generic;
using Xamarin.Forms;
using CocosSharp;

namespace Sampleapp
{
	public class MyCocosSharpView : CocosSharpView 
	{
	}

	public class MainPage : ContentPage
	{
		MyCocosSharpView gameView;

		public MainPage ()
		{
			gameView = new MyCocosSharpView () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				// Set the game world dimensions
				DesignResolution = new Size (1080, 1920),
				// Set the method to call once the view has been initialised
				ViewCreated = LoadGame
			};

			Content = gameView;
		}

		protected override void OnDisappearing ()
		{
			if (gameView != null) {
				gameView.Paused = true;
			}

			base.OnDisappearing ();
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			if (gameView != null)
				gameView.Paused = false;
		}

		void LoadGame (object sender, EventArgs e)
		{
			var nativeGameView = sender as CCGameView;

			if (nativeGameView != null) {
				var contentSearchPaths = new List<string> () { "Fonts", "Sounds" };
				CCSizeI viewSize = nativeGameView.ViewSize;
				CCSizeI designResolution = nativeGameView.DesignResolution;

				// Determine whether to use the high or low def versions of our images
				// Make sure the default texel to content size ratio is set correctly
				// Of course you're free to have a finer set of image resolutions e.g (ld, hd, super-hd)
				if (designResolution.Width < viewSize.Width) {
					contentSearchPaths.Add ("Images/Hd");
					CCSprite.DefaultTexelToContentSizeRatio = 2.0f;
				} else {
					contentSearchPaths.Add ("Images/Ld");
					CCSprite.DefaultTexelToContentSizeRatio = 1.0f;
				}

				nativeGameView.ContentManager.SearchPaths = contentSearchPaths;


				CCScene gameScene = new CCScene (nativeGameView);
			
				var gameLayer = new MainLayer ();
				gameLayer.Schedule ();
				gameScene.AddLayer (gameLayer);
				nativeGameView.RunWithScene (gameScene);
			}
		}
	}
}