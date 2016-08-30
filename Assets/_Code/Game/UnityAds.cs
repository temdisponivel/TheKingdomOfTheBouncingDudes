using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

namespace BounceDudes{

	public class UnityAds : MonoBehaviour {

		protected static float _pseudoModifier = 0f;

		public static void CallAd(){
			if (Random.Range (0f + _pseudoModifier, 1f) >= 0.6f) {
				_pseudoModifier = 0;
				ShowAd ();
			}
			else {
				_pseudoModifier += 0.1f;
			}
				
		}

		public static void ShowAd(){
			if (Advertisement.IsReady ()) {
				Advertisement.Show();
			}
		}
			

		private static void ShowRewardedAd(){
			if (Advertisement.IsReady ("rewardedVideoZone")) {
				var options = new ShowOptions { resultCallback = HandleShowResult };
				Advertisement.Show ("rewardedVideoZone", options);
			}
		}

		private static void HandleShowResult(ShowResult result){
			switch (result) {
			case ShowResult.Finished:
				// Give something
				break;

			case ShowResult.Skipped:
				Debug.Log ("The ad was skipped before reaching the end.");
				break;

			case ShowResult.Failed:
				Debug.LogError("The ad failed to be shown");
				break;
			}

		}
			
	}
}
