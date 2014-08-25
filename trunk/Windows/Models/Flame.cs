namespace Xyrus.Apophysis.Windows.Models
{
	[PublicAPI]
	public class Flame
	{
		private readonly TransformCollection mTransforms;

		public Flame()
		{
			mTransforms = new TransformCollection(this);
		}

		public TransformCollection Transforms
		{
			get { return mTransforms; }
		}
	}
}