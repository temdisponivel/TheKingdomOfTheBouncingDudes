using System;
using UnityEngine;

namespace ES3Types
{
	[ES3PropertiesAttribute("AvailableSoldierInstanceIdById", "Levels", "SoldierNames", "UnleckedAchivments", "CurrentDayTimeSequence")]
	public class ES3Type_GameInfomation : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3Type_GameInfomation() : base(typeof(Assets.Code.Game.GameInfomation)){ Instance = this; }

		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Assets.Code.Game.GameInfomation)obj;
			
			writer.WriteProperty("AvailableSoldierInstanceIdById", instance.AvailableSoldierInstanceIdById);
			writer.WriteProperty("Levels", instance.Levels);
			writer.WriteProperty("SoldierNames", instance.SoldierNames);
			writer.WriteProperty("UnleckedAchivments", instance.UnleckedAchivments);
			writer.WriteProperty("CurrentDayTimeSequence", instance.CurrentDayTimeSequence);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Assets.Code.Game.GameInfomation)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "AvailableSoldierInstanceIdById":
						instance.AvailableSoldierInstanceIdById = reader.Read<System.Collections.Generic.Dictionary<System.Int32, System.Collections.Generic.List<System.Int32>>>();
						break;
					case "Levels":
						instance.Levels = reader.Read<System.Collections.Generic.Dictionary<Assets._Code.Game.LevelId, BounceDudes.LevelInformation>>();
						break;
					case "SoldierNames":
						instance.SoldierNames = reader.Read<System.Collections.Generic.Dictionary<System.Int32, System.Collections.Generic.List<System.String>>>();
						break;
					case "UnleckedAchivments":
						instance.UnleckedAchivments = reader.Read<System.Collections.Generic.List<BounceDudes.AchivmentId>>();
						break;
					case "CurrentDayTimeSequence":
						instance.CurrentDayTimeSequence = reader.Read<BounceDudes.DayTimeSequence>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Assets.Code.Game.GameInfomation();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}

	public class ES3Type_GameInfomationArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_GameInfomationArray() : base(typeof(Assets.Code.Game.GameInfomation[]), ES3Type_GameInfomation.Instance)
		{
			Instance = this;
		}
	}
}