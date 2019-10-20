using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("slug", "questName", "description", "goal", "completed", "itemRewards")]
	public class ES3Type_Quest : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3Type_Quest() : base(typeof(QuestSystem.Quest))
		{
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (QuestSystem.Quest)obj;
			
			writer.WriteProperty("slug", instance.slug, ES3Type_string.Instance);
			writer.WriteProperty("questName", instance.questName, ES3Type_string.Instance);
			writer.WriteProperty("description", instance.description, ES3Type_string.Instance);
			writer.WriteProperty("goal", instance.goal);
			writer.WriteProperty("completed", instance.completed, ES3Type_bool.Instance);
			writer.WriteProperty("itemRewards", instance.itemRewards);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (QuestSystem.Quest)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "slug":
						instance.slug = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "questName":
						instance.questName = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "description":
						instance.description = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "goal":
						instance.goal = reader.Read<QuestSystem.Goal>();
						break;
					case "completed":
						instance.completed = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "itemRewards":
						instance.itemRewards = reader.Read<System.Collections.Generic.List<System.String>>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}

	public class ES3Type_QuestArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_QuestArray() : base(typeof(QuestSystem.Quest[]), ES3Type_Quest.Instance)
		{
			Instance = this;
		}
	}
}