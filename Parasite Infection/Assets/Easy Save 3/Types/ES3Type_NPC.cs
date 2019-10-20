using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("movementDirections", "spawnPosition", "wander", "questName", "quest", "dialogData", "questCompletedDialogData", "dialog")]
	public class ES3Type_NPC : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3Type_NPC() : base(typeof(NPC))
		{
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (NPC)obj;
			
			writer.WritePrivateField("movementDirections", instance);
			writer.WritePrivateField("spawnPosition", instance);
			writer.WritePrivateField("wander", instance);
			writer.WritePrivateField("questName", instance);
			writer.WritePrivateFieldByRef("quest", instance);
			writer.WritePrivateFieldByRef("dialogData", instance);
			writer.WritePrivateFieldByRef("questCompletedDialogData", instance);
			writer.WritePrivateFieldByRef("dialog", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (NPC)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "movementDirections":
					reader.SetPrivateField("movementDirections", reader.Read<UnityEngine.Vector2[]>(), instance);
					break;
					case "spawnPosition":
					reader.SetPrivateField("spawnPosition", reader.Read<UnityEngine.Vector2>(), instance);
					break;
					case "wander":
					reader.SetPrivateField("wander", reader.Read<System.Boolean>(), instance);
					break;
					case "questName":
					reader.SetPrivateField("questName", reader.Read<System.String>(), instance);
					break;
					case "quest":
					reader.SetPrivateField("quest", reader.Read<QuestSystem.Quest>(), instance);
					break;
					case "dialogData":
					reader.SetPrivateField("dialogData", reader.Read<DialogData>(), instance);
					break;
					case "questCompletedDialogData":
					reader.SetPrivateField("questCompletedDialogData", reader.Read<DialogData>(), instance);
					break;
					case "dialog":
					reader.SetPrivateField("dialog", reader.Read<DialogPanel>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}

	public class ES3Type_NPCArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_NPCArray() : base(typeof(NPC[]), ES3Type_NPC.Instance)
		{
			Instance = this;
		}
	}
}