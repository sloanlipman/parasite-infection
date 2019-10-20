using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("enemyList", "partyMembers", "activePartyMembers", "abilityList")]
	public class ES3Type_CharacterDatabase : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3Type_CharacterDatabase() : base(typeof(BattleSystem.CharacterDatabase))
		{
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (BattleSystem.CharacterDatabase)obj;
			
			writer.WriteProperty("enemyList", instance.enemyList);
			writer.WriteProperty("partyMembers", instance.partyMembers);
			writer.WriteProperty("activePartyMembers", instance.activePartyMembers);
			writer.WriteProperty("abilityList", instance.abilityList);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (BattleSystem.CharacterDatabase)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "enemyList":
						instance.enemyList = reader.Read<System.Collections.Generic.List<BattleSystem.Enemy>>();
						break;
					case "partyMembers":
						instance.partyMembers = reader.Read<System.Collections.Generic.List<BattleSystem.PartyMember>>();
						break;
					case "activePartyMembers":
						instance.activePartyMembers = reader.Read<System.Collections.Generic.List<System.String>>();
						break;
					case "abilityList":
						instance.abilityList = reader.Read<System.Collections.Generic.Dictionary<System.String, BattleSystem.Ability>>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}

	public class ES3Type_CharacterDatabaseArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_CharacterDatabaseArray() : base(typeof(BattleSystem.CharacterDatabase[]), ES3Type_CharacterDatabase.Instance)
		{
			Instance = this;
		}
	}
}