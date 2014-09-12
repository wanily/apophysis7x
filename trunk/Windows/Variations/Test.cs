using System;
using Xyrus.Apophysis.Calculation;

namespace Xyrus.Apophysis.Variations
{
	class Test : ExtendedVariation
	{
		private double mVariable;

		public override double GetVariable(string name)
		{
			if (string.Equals(name, "test_var", StringComparison.InvariantCultureIgnoreCase))
			{
				return mVariable;
			}

			return base.GetVariable(name);
		}
		public override double SetVariable(string name, double value)
		{
			if (string.Equals(name, "test_var", StringComparison.InvariantCultureIgnoreCase))
			{
				return mVariable = System.Math.Round(value * 2.0) / 2.0;
			}

			return base.SetVariable(name, value);
		}
		public override double ResetVariable(string name)
		{
			if (string.Equals(name, "test_var", StringComparison.InvariantCultureIgnoreCase))
			{
				return mVariable = 0.5;
			}

			return base.ResetVariable(name);
		}

		public override int GetVariableCount()
		{
			return 1;
		}
		public override string GetVariableNameAt(int index)
		{
			if (index == 0)
			{
				return "test_var";
			}

			return base.GetVariableNameAt(index);
		}

		public override void Calculate(IterationData data)
		{
			data.PostX += Weight * mVariable*data.PreX;
			data.PostY += Weight * mVariable/(System.Math.Abs(data.PreY) < double.Epsilon ? double.Epsilon : data.PreY);
			data.PostZ += Weight * data.PreZ;
		}
	}
}