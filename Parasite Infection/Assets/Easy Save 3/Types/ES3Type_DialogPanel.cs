using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("dialogText", "dialogPanel", "dialog", "dialogIndex")]
	public class ES3Type_DialogPanel : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3Type_DialogPanel() : base(typeof(DialogPanel))
		{
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (DialogPanel)obj;
			
			writer.WritePrivateFieldByRef("dialogText", instance);
			writer.WritePrivateFieldByRef("dialogPanel", instance);
			writer.WritePrivateField("dialog", instance);
			writer.WritePrivateField("dialogIndex", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (DialogPanel)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "dialogText":
					reader.SetPrivateField("dialogText", reader.Read<UnityEngine.UI.Text>(), instance);
					break;
					case "dialogPanel":
					reader.SetPrivateField("dialogPanel", reader.Read<UnityEngine.GameObject>(), instance);
					break;
					case "dialog":
					reader.SetPrivateField("dialog", reader.Read<System.String[]>(), instance);
					break;
					case "dialogIndex":
					reader.SetPrivateField("dialogIndex", reader.Read<System.Int32>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}

	public class ES3Type_DialogPanelArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_DialogPanelArray() : base(typeof(DialogPanel[]), ES3Type_DialogPanel.Instance)
		{
			Instance = this;
		}
	}
}