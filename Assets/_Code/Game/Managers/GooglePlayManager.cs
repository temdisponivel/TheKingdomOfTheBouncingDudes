using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


namespace BounceDudes
{
	/// <summary>
	/// Handles all the Google Play actions.
	/// </summary>
	public class GooglePlayManager {

		public int _totalMonstersDefeated = 0;
		public void AddMonsterDefeated(){

			var incrementValue = 10;

			GameManager.GPManagerInstance._totalMonstersDefeated += 1;

			if (GameManager.GPManagerInstance._totalMonstersDefeated % incrementValue == 0) {
				GameManager.GPManagerInstance.IncrementAchievement (GPGSIds.achievement_monster_repelent_bronze, incrementValue);
				GameManager.GPManagerInstance.IncrementAchievement (GPGSIds.achievement_monster_repelent_silver, incrementValue);
				GameManager.GPManagerInstance.IncrementAchievement (GPGSIds.achievement_monster_repelent_gold, incrementValue);
				GameManager.GPManagerInstance.IncrementAchievement (GPGSIds.achievement_monster_repelent_master, incrementValue);
			}
		}

		public void AuthenticateUser(){

			PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder ().Build ();
			PlayGamesPlatform.InitializeInstance (config);
			PlayGamesPlatform.DebugLogEnabled = true;
			PlayGamesPlatform.Activate ();

			//authenticate user on Google Play
			Social.localUser.Authenticate ((bool success) => {
				if (success)
					Debug.Log("Logged Successfully");
				else
					Debug.Log("Authentication Failed");

			});

		}

		public void WriteInLeaderboard(int value, string leaderboardID){
			Social.ReportScore (value, leaderboardID, (bool success) => {
				
			});
		}


		public void UnlockAchievement(string achievementID){
			Social.ReportProgress (achievementID, 100f, (bool success) => {
				
			});
		}


		public void IncrementAchievement (string achievementID, int value){
			PlayGamesPlatform.Instance.IncrementAchievement (achievementID, value, (bool success) => {
			
			});
		}


		public void CallLeaderboardUI(){
			Social.ShowLeaderboardUI ();
		}

		public void CallAchievementsUI(){
			Social.ShowAchievementsUI ();
		}
		

	}
}
