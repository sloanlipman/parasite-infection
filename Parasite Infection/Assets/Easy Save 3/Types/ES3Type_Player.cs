using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("body", "sprite", "colliding", "animator", "IsMoving")]
	public class ES3Type_Player : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3Type_Player() : base(typeof(Player))
		{
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (Player)obj;
			
			writer.WritePrivateFieldByRef("body", instance);
			writer.WritePrivateFieldByRef("sprite", instance);
			writer.WritePrivateField("colliding", instance);
			writer.WritePrivateFieldByRef("animator", instance);
			writer.WriteProperty("IsMoving", instance.IsMoving, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Player)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "body":
					reader.SetPrivateField("body", reader.Read<UnityEngine.Rigidbody2D>(), instance);
					break;
					case "sprite":
					reader.SetPrivateField("sprite", reader.Read<UnityEngine.SpriteRenderer>(), instance);
					break;
					case "colliding":
					reader.SetPrivateField("colliding", reader.Read<System.Boolean>(), instance);
					break;
					case "animator":
					reader.SetPrivateField("animator", reader.Read<UnityEngine.Animator>(), instance);
					break;
					case "IsMoving":
						instance.IsMoving = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}

	public class ES3Type_PlayerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_PlayerArray() : base(typeof(Player[]), ES3Type_Player.Instance)
		{
			Instance = this;
		}
	}
}