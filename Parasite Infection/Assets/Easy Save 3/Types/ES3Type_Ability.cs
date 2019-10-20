using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("abilityName", "power", "energyCost", "abilityType", "targetPosition")]
	public class ES3Type_Ability : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3Type_Ability() : base(typeof(BattleSystem.Ability))
		{
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (BattleSystem.Ability)obj;
			
			writer.WriteProperty("abilityName", instance.abilityName, ES3Type_string.Instance);
			writer.WriteProperty("power", instance.power, ES3Type_int.Instance);
			writer.WriteProperty("energyCost", instance.energyCost, ES3Type_int.Instance);
			writer.WriteProperty("abilityType", instance.abilityType);
			writer.WritePrivateField("targetPosition", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (BattleSystem.Ability)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "abilityName":
						instance.abilityName = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "power":
						instance.power = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "energyCost":
						instance.energyCost = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "abilityType":
						instance.abilityType = reader.Read<BattleSystem.Ability.AbilityType>();
						break;
					case "targetPosition":
					reader.SetPrivateField("targetPosition", reader.Read<UnityEngine.Vector3>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}

	public class ES3Type_AbilityArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_AbilityArray() : base(typeof(BattleSystem.Ability[]), ES3Type_Ability.Instance)
		{
			Instance = this;
		}
	}
}