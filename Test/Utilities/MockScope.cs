using System;
using Microsoft.Practices.Unity;
using Moq;

namespace Xyrus.Apophysis.Test.Utilities
{
	public class MockScope<T> : IDisposable where T: class
	{
		private readonly Mock<T> mMock;

		public MockScope(MockBehavior behavior = MockBehavior.Strict)
		{
			mMock = new Mock<T>(behavior);
			ApophysisApplication.Container.RegisterInstance(typeof (T), mMock.Object, new PerResolveLifetimeManager());
		}
		public Mock<T> Mock
		{
			get { return mMock; }
		}
		public void Dispose()
		{
			mMock.VerifyAll();
		}
	}
}
