using System;

namespace Xyrus.Apophysis.Variations
{
	[PublicAPI]
	public class Variable
	{
		public Variable(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			Name = name;
			Value = 0.0;
		}

		public string Name { get; private set; }
		public double Value { get; set; }
	}
}