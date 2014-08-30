using System.ComponentModel.Design.Serialization;

namespace Xyrus.Apophysis.Windows.Controls
{
	class EditorSettingsSerializer : CodeDomSerializer
	{
		public override object Deserialize(IDesignerSerializationManager manager, object codeObject)
		{
			var baseClassSerializer =
				(CodeDomSerializer) manager.GetSerializer(typeof (EditorSettings).BaseType, typeof (CodeDomSerializer));
			return baseClassSerializer.Deserialize(manager, codeObject);
		}

		public override object Serialize(IDesignerSerializationManager manager, object value)
		{
			var baseClassSerializer =
				(CodeDomSerializer) manager.GetSerializer(typeof (EditorSettings).BaseType, typeof (CodeDomSerializer));
			var codeObject = baseClassSerializer.Serialize(manager, value);

			return codeObject;
		}
	}
}