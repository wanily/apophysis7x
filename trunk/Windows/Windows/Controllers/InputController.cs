using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	public class InputController
	{
		public void HandleKeyPressForNumericTextBox(KeyPressEventArgs args)
		{
			char[] chars =
			{
				'.', ',', '-', 'e', 'E', '+',
				'0', '1', '2', '3', '4', '5',
				'6', '7', '8', '9'
			};

			if (!chars.Contains(args.KeyChar))
			{
				args.Handled = true;
			}
		}

		public static CultureInfo Culture
		{
			get { return CultureInfo.InvariantCulture; }
		}
		public static string DefaultFormat
		{
			get { return "#,###,##0.000"; }
		}
		public static string PreciseFormat
		{
			get { return "#,###,##0.000000"; }
		}

		public static event KeyEventHandler GlobalKeyPressed;
		public static void HandleKeyboardInput(Form window, Keys keyData)
		{
			if (GlobalKeyPressed != null)
				GlobalKeyPressed(window, new KeyEventArgs(keyData));
		}
	}
}
