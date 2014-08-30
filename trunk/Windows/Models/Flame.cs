namespace Xyrus.Apophysis.Windows.Models
{
	[PublicAPI]
	public class Flame
	{
		private readonly IteratorCollection mIterators;

		public Flame()
		{
			mIterators = new IteratorCollection(this);
		}

		public IteratorCollection Iterators
		{
			get { return mIterators; }
		}
	}
}