using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xyrus.Apophysis.Test.Utilities;
using Xyrus.Apophysis.Windows.Controllers;
using Xyrus.Apophysis.Windows.Interfaces.Controllers;

namespace Xyrus.Apophysis.Test
{
	[TestClass]
	public class AutosaveControllerTest
	{
		[TestCleanup]
		public void Cleanup()
		{
			ApophysisApplication.ClearContainer();
		}

		[TestMethod]
		public void DisposeTest()
		{
			var instance = new AutosaveController();

			using (var main = new MockScope<IMainController>())
			using (var editor = new MockScope<IEditorController>())
			using (var flameProperties = new MockScope<IFlamePropertiesController>())
			{
				main.Mock.SetupGet(x => x.EditorController).Returns(editor.Object);
				main.Mock.SetupGet(x => x.FlamePropertiesController).Returns(flameProperties.Object);

				// todo x: https://code.google.com/p/moq/issues/detail?id=100
				//
				//editor.Mock.Verify(v => v.FlameChanged -= It.IsAny<EventHandler>());
				//flameProperties.Mock.Verify(v => v.FlameChanged -= It.IsAny<EventHandler>());

				instance.Dispose();
			}
		}
	}
}