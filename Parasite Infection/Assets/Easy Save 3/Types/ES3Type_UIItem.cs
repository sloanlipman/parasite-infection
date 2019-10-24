using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("item", "isCraftingSlot", "isCraftingResultSlot", "isPlayerEquipmentSlot")]
	public class ES3Type_UIItem : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3Type_UIItem() : base(typeof(UIItem))
		{
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (UIItem)obj;
			
			writer.WriteProperty("item", instance.item, ES3Type_Item.Instance);
			writer.WriteProperty("isCraftingSlot", instance.isCraftingSlot, ES3Type_bool.Instance);
			writer.WriteProperty("isCraftingResultSlot", instance.isCraftingResultSlot, ES3Type_bool.Instance);
			writer.WriteProperty("isPlayerEquipmentSlot", instance.isPlayerEquipmentSlot, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (UIItem)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "item":
						instance.item = reader.Read<Item>(ES3Type_Item.Instance);
						break;
					case "isCraftingSlot":
						instance.isCraftingSlot = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "isCraftingResultSlot":
						instance.isCraftingResultSlot = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "isPlayerEquipmentSlot":
						instance.isPlayerEquipmentSlot = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}

	public class ES3Type_UIItemArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_UIItemArray() : base(typeof(UIItem[]), ES3Type_UIItem.Instance)
		{
			Instance = this;
		}
	}
}