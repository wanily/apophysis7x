using System;
using System.Collections.Generic;
using System.Linq;

namespace Xyrus.Apophysis.Windows.Math
{
	[PublicAPI]
	public class Polygon
	{
		private readonly Vector2[] mVertices;

		public Polygon(IEnumerable<Vector2> vertices)
		{
			if (vertices == null) throw new ArgumentNullException("vertices");

			mVertices = vertices.ToArray();

			if (mVertices.Length <= 2)
			{
				throw new ArgumentOutOfRangeException("vertices", "A polygon must contain at least three vertices");
			}
		}

		public int VertexCount
		{
			get { return mVertices.Length; }
		}
		public Vector2 this[int index]
		{
			get { return mVertices[index]; }
		}

		public bool IsOnSurface(Vector2 point)
		{
			int i, j;
			bool hit = false;

			for (i = 0, j = VertexCount - 1; i < VertexCount; j = i++)
			{
				if (((mVertices[i].Y > point.Y) != (mVertices[j].Y > point.Y)) && (point.X < (mVertices[j].X - mVertices[i].X)*(point.Y - mVertices[i].Y)/(mVertices[j].Y - mVertices[i].Y) + mVertices[i].X)) 
					hit = !hit;
			}

			return hit;
		}
	}
}