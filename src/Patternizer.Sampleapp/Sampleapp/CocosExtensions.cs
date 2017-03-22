using System;
using System.Threading.Tasks;
using CocosSharp;
using System.Collections.Generic;

namespace Sampleapp
{
	public static class CocosExtensions
	{
		/// <summary>
		/// Runs an action so that it can be awaited.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="actions">Actions.</param>
		public static Task<bool> RunActionsAsync(this CCNode node, params CCFiniteTimeAction[] actions)
		{
			var tcs = new TaskCompletionSource<bool> ();
			var allActions = new List<CCFiniteTimeAction> ();
			allActions.AddRange (actions);
			allActions.Add (new CCCallFunc (() => tcs.TrySetResult (true)));
			node.RunActions (allActions.ToArray());
			return tcs.Task;
		}
	}
}