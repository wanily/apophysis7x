using System;
using System.Collections.Generic;
using Moq;

namespace Xyrus.Apophysis.Test.Utilities
{
	public class MockChain : IDisposable
	{
		private MockChain(MockChain root)
		{
			Scopes = root == null ? new List<IDisposable>() : root.Scopes;
		}

		private List<IDisposable> Scopes { get; set; }

		public static MockChain Mock<T>(out Mock<T> mock, MockBehavior behavior = MockBehavior.Strict) where T : class
		{
			return new MockChain(null).And(out mock, behavior);
		}

		public MockChain And<T>(out Mock<T> mock, MockBehavior behavior = MockBehavior.Strict) where T: class
		{
			var scope = new MockScope<T>(behavior);
			Scopes.Add(scope);
			mock = scope.Mock;
			return this;
		}
		public void Dispose()
		{
			Scopes.ForEach(x => x.Dispose());
		}
	}
}