using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("id", "itemName", "description", "icon", "stats")]
	public class ES3Type_Item : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3Type_Item() : base(typeof(Item)){ Instance = this; }

		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Item)obj;
			
			writer.WriteProperty("id", instance.id, ES3Type_int.Instance);
			writer.WriteProperty("itemName", instance.itemName, ES3Type_string.Instance);
			writer.WriteProperty("description", instance.description, ES3Type_string.Instance);
			writer.WritePropertyByRef("icon", instance.icon);
			writer.WriteProperty("stats", instance.stats);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Item)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "id":
						instance.id = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "itemName":
						instance.itemName = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "description":
						instance.description = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "icon":
						instance.icon = reader.Read<UnityEngine.Sprite>(ES3Type_Sprite.Instance);
						break;
					case "stats":
						instance.stats = reader.Read<System.Collections.Generic.Dictionary<System.String, System.Int32>>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Item();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}

	public class ES3Type_ItemArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_ItemArray() : base(typeof(Item[]), ES3Type_Item.Instance)
		{
			Instance = this;
		}
	}
}