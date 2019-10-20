using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("body", "movementSpeed", "sprite")]
	public class ES3Type_Character : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3Type_Character() : base(typeof(Character))
		{
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (Character)obj;
			
			writer.WritePrivateFieldByRef("body", instance);
			writer.WritePrivateField("movementSpeed", instance);
			writer.WritePrivateFieldByRef("sprite", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Character)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "body":
					reader.SetPrivateField("body", reader.Read<UnityEngine.Rigidbody2D>(), instance);
					break;
					case "movementSpeed":
					reader.SetPrivateField("movementSpeed", reader.Read<System.Single>(), instance);
					break;
					case "sprite":
					reader.SetPrivateField("sprite", reader.Read<UnityEngine.SpriteRenderer>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}

	public class ES3Type_CharacterArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_CharacterArray() : base(typeof(Character[]), ES3Type_Character.Instance)
		{
			Instance = this;
		}
	}
}