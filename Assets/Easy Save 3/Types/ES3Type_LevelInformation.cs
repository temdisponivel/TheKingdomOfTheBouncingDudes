using System;
using UnityEngine;

namespace ES3Types
{
	[ES3PropertiesAttribute("LevelId", "EnemiesKilled", "ShootCount", "Finished", "ChallengesCompleted", "Star")]
	public class ES3Type_LevelInformation : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3Type_LevelInformation() : base(typeof(BounceDudes.LevelInformation)){ Instance = this; }

		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (BounceDudes.LevelInformation)obj;
			
			writer.WriteProperty("LevelId", instance.LevelId, ES3Type_enum.Instance);
			writer.WriteProperty("EnemiesKilled", instance.EnemiesKilled, ES3Type_int.Instance);
			writer.WriteProperty("ShootCount", instance.ShootCount, ES3Type_int.Instance);
			writer.WriteProperty("Finished", instance.Finished, ES3Type_bool.Instance);
			writer.WriteProperty("ChallengesCompleted", instance.ChallengesCompleted);
			writer.WriteProperty("Star", instance.Star, ES3Type_int.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (BounceDudes.LevelInformation)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "LevelId":
						instance.LevelId = reader.Read<Assets._Code.Game.LevelId>(ES3Type_enum.Instance);
						break;
					case "EnemiesKilled":
						instance.EnemiesKilled = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "ShootCount":
						instance.ShootCount = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Finished":
						instance.Finished = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "ChallengesCompleted":
						instance.ChallengesCompleted = reader.Read<System.Collections.Generic.Dictionary<BounceDudes.Challenge, System.Int32[]>>();
						break;
					case "Star":
						instance.Star = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new BounceDudes.LevelInformation();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}

	public class ES3Type_LevelInformationArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_LevelInformationArray() : base(typeof(BounceDudes.LevelInformation[]), ES3Type_LevelInformation.Instance)
		{
			Instance = this;
		}
	}
}