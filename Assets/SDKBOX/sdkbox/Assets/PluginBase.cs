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
using Sdkbox;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;

namespace Sdkbox
{
	[Serializable]
	public abstract class PluginBase<T> : MonoBehaviour where T : PluginBase<T>
	{
		// iOS requires a static callback due to AOT compilation.
		// We cache the T instance to redirect the callback to the instance.
		protected static T _this;

		#if !UNITY_EDITOR
		#if UNITY_ANDROID
		// we need to access the Unity java player to run methods
		// on the UI thread, so we cache this at initialization time.
		protected static AndroidJavaClass _player;
		#endif
		#endif // !UNITY_EDITOR

		// in order to ensure execution order out of the box,
		// we always lazy init in all API calls.
		protected bool _have_lazy_init = false;
		protected void _lazy_init()
		{
			if (false == _have_lazy_init)
			{
				_have_lazy_init = true;
				#if !UNITY_EDITOR
				#if UNITY_ANDROID
				PluginBase<T>._player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				#endif
				#endif // !UNITY_EDITOR
				init();
			}
		}

		protected void Awake()
		{
			// This may not be needed, but the object will be initialized twice without it.
			DontDestroyOnLoad(transform.gameObject);

			// cache the instance for the callbacks
			_this = (T)this;
		}

		protected void Start()
		{
			_lazy_init();
		}

		protected virtual void init()
		{
		    throw new NotImplementedException();
		}

		protected Json newJsonObject()
		{
			Dictionary<string, Json> o = new Dictionary<string, Json>();
			return new Json(o);
		}
	}
}
