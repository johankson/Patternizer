using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Patternizer;

namespace Patternizer.Test
{
	internal static class PathsForTesting
	{
		public static List<Line> WideRectangle1
		{
			get {
				// TODO This is based on y being on the lower side of the screen
				return new List<Line> () {
					new Line (10, 10, 300, 9),
					new Line (300, 9, 293, -60),
					new Line (293, -60, 7, -67),
					new Line (7, -67, 8, 7)
				};
			}
		}

		public static List<Line> TallRectangle
		{
			get
			{
				// TODO This is based on y being on the lower side of the screen
				return new List<Line>() {
					new Line (10, 10, 70, 9),
					new Line (70, 9, 70, -193),
					new Line (79, -193, 7, -193),
					new Line (7, -193, 8, 7)
				};
			}
		}

		public static List<Line> Image
		{
			get {
				// TODO This is based on y being on the lower side of the screen
				return new List<Line> () {
					new Line (10, 10, 200, -200),
					new Line (200, 6, 10, -200)
				};
			}
		}

        public static List<Line> InverseImage
        {
            get
            {
                // TODO This is based on y being on the lower side of the screen
                return new List<Line>() {
                    new Line (200, 6, 10, -200),
                    new Line (10, 10, 200, -200)
                };
            }
        }

        public static List<Line> JigSawPath
		{
			get {
				// TODO This is based on y being on the lower side of the screen
				return new List<Line> () {
					new Line (10, 10, 200, 9),      // Right
					new Line (200, 9, 20, -50),     // Left & Down 
					new Line (20, -50, 187, -64),   // Right
					new Line (187, -64, 8, -98),    // Left & Down
					new Line (9, -63, 200, -96)     // Right
				};
			}
		}
	}
}
