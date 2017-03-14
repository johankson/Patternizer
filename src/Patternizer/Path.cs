using System;
using System.Collections.Generic;

namespace Patternizer
{
	public class Path
	{
		public static List<Line> Parse(string path)
		{
			if (path.Contains("Line"))
			{
				return ParseRowBasedPath(path);
			}

			if (path.Contains(">"))
			{
				return ParseLinkedList(path);
			}

			throw new Exception("The format of the path is not recognized");
		}

		static List<Line> ParseLinkedList(string path)
		{
			var result = new List<Line>();

			// 10,10->10,40->30,40

			// Remove white space
			path = path.Replace(" ", "").Trim();

			var superarr = path.Split('|');

			foreach (var super in superarr)
			{
				var arr = super.Split(new string[] { "->" }, StringSplitOptions.None);

				if (arr.Length < 2)
					throw new Exception("You have to add at least 2 positions");

				for (int index = 0; index < arr.Length - 1; index++)
				{
					var p1 = ResolvePoint(arr[index]);
					var p2 = ResolvePoint(arr[index + 1]);
					result.Add(new Line(p1, p2));
				}
			}

			return result;
		}

		static List<Line> ParseRowBasedPath(string path)
		{
			var result = new List<Line>();

			// Remove white space
			path = path.Replace(" ", "").Trim();

			var rows = path.Split(Environment.NewLine.ToCharArray());

			foreach (var row in rows)
			{
				var data = row.Replace("Line", "");
				var arr = data.Split(new string[] { "->" }, StringSplitOptions.None);
				var p1 = ResolvePoint(arr[0]);
				var p2 = ResolvePoint(arr[1]);
				result.Add(new Line(p1, p2));
			}

			return result;
		}

		static Point ResolvePoint(string point)
		{
			var arr = point.Split(",".ToCharArray());
			return new Point(float.Parse(arr[0]), float.Parse(arr[1]));
		}
	}
}
