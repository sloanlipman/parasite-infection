using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute()]
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
			
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Player)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
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