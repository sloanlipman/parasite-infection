using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("dialog")]
	public class ES3Type_DialogData : ES3ScriptableObjectType
	{
		public static ES3Type Instance = null;

		public ES3Type_DialogData() : base(typeof(DialogData)){ Instance = this; }

		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
			var instance = (DialogData)obj;
			
			writer.WriteProperty("dialog", instance.dialog, ES3Type_StringArray.Instance);
		}

		protected override void ReadScriptableObject<T>(ES3Reader reader, object obj)
		{
			var instance = (DialogData)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "dialog":
						instance.dialog = reader.Read<System.String[]>(ES3Type_StringArray.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}

	public class ES3Type_DialogDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_DialogDataArray() : base(typeof(DialogData[]), ES3Type_DialogData.Instance)
		{
			Instance = this;
		}
	}
}