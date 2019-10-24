using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("inParty", "characterName", "health", "maxHealth", "attackPower", "defensePower", "energyPoints", "maxEnergyPoints", "speed", "abilities", "abilitiesList", "level", "experience", "equipment")]
	public class ES3Type_PartyMember : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3Type_PartyMember() : base(typeof(BattleSystem.PartyMember))
		{
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (BattleSystem.PartyMember)obj;
			
			writer.WritePrivateField("inParty", instance);
			writer.WriteProperty("characterName", instance.characterName, ES3Type_string.Instance);
			writer.WriteProperty("health", instance.health, ES3Type_int.Instance);
			writer.WriteProperty("maxHealth", instance.maxHealth, ES3Type_int.Instance);
			writer.WriteProperty("attackPower", instance.attackPower, ES3Type_int.Instance);
			writer.WriteProperty("defensePower", instance.defensePower, ES3Type_int.Instance);
			writer.WriteProperty("energyPoints", instance.energyPoints, ES3Type_int.Instance);
			writer.WriteProperty("maxEnergyPoints", instance.maxEnergyPoints, ES3Type_int.Instance);
			writer.WriteProperty("speed", instance.speed, ES3Type_int.Instance);
			writer.WriteProperty("abilities", instance.abilities);
			writer.WriteProperty("abilitiesList", instance.abilitiesList);
			writer.WriteProperty("level", instance.level, ES3Type_int.Instance);
			writer.WriteProperty("experience", instance.experience, ES3Type_int.Instance);
			writer.WriteProperty("equipment", instance.equipment);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (BattleSystem.PartyMember)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "inParty":
					reader.SetPrivateField("inParty", reader.Read<System.Boolean>(), instance);
					break;
					case "characterName":
						instance.characterName = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "health":
						instance.health = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "maxHealth":
						instance.maxHealth = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "attackPower":
						instance.attackPower = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "defensePower":
						instance.defensePower = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "energyPoints":
						instance.energyPoints = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "maxEnergyPoints":
						instance.maxEnergyPoints = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "speed":
						instance.speed = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "abilities":
						instance.abilities = reader.Read<System.Collections.Generic.List<BattleSystem.Ability>>();
						break;
					case "abilitiesList":
						instance.abilitiesList = reader.Read<System.Collections.Generic.List<System.String>>();
						break;
					case "level":
						instance.level = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "experience":
						instance.experience = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "equipment":
						instance.equipment = reader.Read<Item[]>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}

	public class ES3Type_PartyMemberArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_PartyMemberArray() : base(typeof(BattleSystem.PartyMember[]), ES3Type_PartyMember.Instance)
		{
			Instance = this;
		}
	}
}