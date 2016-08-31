using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Assets._Code.Game;


namespace BounceDudes
{
	/// <summary>
	/// Handles all the Google Play actions.
	/// </summary>
	public class GooglePlayManager {
	
		protected bool _userLogged = false;
		public bool UserLogged { get { return _userLogged; } }

		public void CheckKingTruePower(){
			var info = GameManager.Instance.LevelsInformation[LevelId.FIFTEEN];
			if (info.GetCountChallengesCompleted == 3) {
				this.UnlockAchievement (GPGSIds.achievement_bob_is_no_more);
			}
		}


		public void CheckFullComplete(){
			if (GameManager.Instance.LevelsInformation.Count >= 16) {
				foreach (LevelInformation levelInfo in GameManager.Instance.LevelsInformation.Values) {
					if (levelInfo.GetCountChallengesCompleted < 3 || levelInfo.Star < 3) {
						return;
					}

					this.UnlockAchievement (GPGSIds.achievement_completionist);
				}
			}
				
		}

		public void CommitMonstersDefeated(int increment){
			this.IncrementAchievement (GPGSIds.achievement_monster_repelent_bronze, increment);
			this.IncrementAchievement (GPGSIds.achievement_monster_repelent_silver, increment);
			this.IncrementAchievement (GPGSIds.achievement_monster_repelent_gold, increment);
			this.IncrementAchievement (GPGSIds.achievement_monster_repelent_master, increment);
		}

		public void AuthenticateUser(){

			PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder ().Build ();
			PlayGamesPlatform.InitializeInstance (config);
			PlayGamesPlatform.DebugLogEnabled = true;
			PlayGamesPlatform.Activate ();

			//authenticate user on Google Play
			Social.localUser.Authenticate ((bool success) => {
				if (success){
					Debug.Log("Logged Successfully");
					_userLogged = true;
				}
				else{
					Debug.Log("Authentication Failed");
					_userLogged = false;
				}

			});

		}


		public void WriteInLeaderboard(int value, string leaderboardID){

			if (!UserLogged)
				return;

			Social.ReportScore (value, leaderboardID, (bool success) => {
				
			});
		}


		public void UnlockAchievement(string achievementID){

			if (!UserLogged)
				return;

			Social.ReportProgress (achievementID, 100f, (bool success) => {
				
			});
		}

		public void RevealAchievement(string achievementID){

			if (!UserLogged)
				return;

			Social.ReportProgress (achievementID, 0f, (bool success) => {

			});
		}

		public void IncrementAchievement (string achievementID, int value){

			if (!UserLogged)
				return;

			PlayGamesPlatform.Instance.IncrementAchievement (achievementID, value, (bool success) => {
			
			});
		}


		public void CallLeaderboardUI(){
			
			if (!UserLogged)
				return;
			
			Social.ShowLeaderboardUI ();
		}

		public void CallAchievementsUI(){
			
			if (!UserLogged)
				return;

			Social.ShowAchievementsUI ();
		}
		

	}
}
