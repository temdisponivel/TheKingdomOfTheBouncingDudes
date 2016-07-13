/*****************************************************************************
Copyright © 2015 SDKBOX.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*****************************************************************************/

using UnityEngine;

namespace Sdkbox
{
	public class SDKBOX : MonoBehaviour
	{
        private static SDKBOX _instance = null;

        public static SDKBOX Instance
        {
            get { return _instance ?? (_instance = SDKBOX.create()); }
        }

    	void Awake()
		{
			// This may not be needed, but the object will be initialized twice without it.
			DontDestroyOnLoad(transform.gameObject);
		}

		public void init()
		{
			// Does nothing, just so that plugins have something to call.
		}

		private static SDKBOX create()
		{
			string name = "__SDKBOX_RT__";
			GameObject go = GameObject.Find(name);
		    if (!go)
		    {
		        go = new GameObject(name);
                go.hideFlags = HideFlags.HideInHierarchy;
		        go.AddComponent<SDKBOX>();
			}

			SDKBOX sdkbox = go.GetComponent<SDKBOX>();

			#if !UNITY_EDITOR
			#if UNITY_ANDROID
			// load libsdkbox.so
			AndroidJavaObject sys = new AndroidJavaObject("java.lang.System");
			sys.CallStatic("loadLibrary", "sdkbox");

			AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
			activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
			{
				// call SDKBox.Init()
				AndroidJavaObject jo = new AndroidJavaObject("com.sdkbox.plugin.SDKBox");
				jo.CallStatic("init", activity);
			}));
			#endif//UNITY_ANDROID
			#endif//!UNITY_EDITOR
			Debug.Log("SDKBOX Initialized.");

			return sdkbox;
		}
    }
}
