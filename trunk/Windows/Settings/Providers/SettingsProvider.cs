using System.Configuration;
using System.Reflection;

namespace Xyrus.Apophysis.Settings.Providers
{
	public class SettingsProvider<T> where T : ApplicationSettingsBase
	{
		private static readonly T mContainer;

		static SettingsProvider()
		{
			var defaultProperty = typeof (T).GetProperty(@"Default", BindingFlags.Static | BindingFlags.Public);
			mContainer = defaultProperty.GetValue(null, null) as T;
		} 
		protected static T Container
		{
			get { return mContainer; }
		}

		public void Serialize()
		{
			mContainer.Save();
		}
	}
}