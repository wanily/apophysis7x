using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Interfaces.Controllers
{
	public interface IIteratorController : IController
	{
		void BuildIteratorComboBox();
		void SelectIterator([CanBeNull] Iterator iterator);

		IIteratorColorController ColorControls { get; }
		IIteratorPointEditController PointControls { get; }
		IIteratorVectorEditController VectorControls { get; }
		IIteratorVariationsController VariationsList { get; }
		IIteratorVariablesController VariablesList { get; }
	}
}