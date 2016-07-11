using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;


namespace BounceDudes
{
	/// <summary>
	/// Handles all the Google Play actions.
	/// </summary>
	public class GooglePlayManager {

		public void AuthenticateUser(){

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
