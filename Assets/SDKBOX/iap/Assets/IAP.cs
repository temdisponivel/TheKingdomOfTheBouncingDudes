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
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using AOT;

namespace Sdkbox
{
	[Serializable]
	public struct ProductDescription
	{
		// Name of the product
		public string name;

		// The product id of an In App Purchase
		public string id;

		// consumable or not
		public bool consumable;
	}

	// Product from SDKBox In App Purchase
	[Serializable]
	public struct Product
	{
		public enum Type {CONSUMABLE, NON_CONSUMABLE};

		// The name specified in sdkbox_config.json
		public string name;
		
		// The product id of an In App Purchase
		public string id;
		
		// Type of iap item
		public Type type;
		
		// The title of the IAP item
		public string title;
		
		// The description of the IAP item
		public string description;
		
		// Price value in float
		public float priceValue;
		
		// Localized price
		public string price;
		
		// price currency code
		public string currencyCode;

        // cyphered payload
        public string receiptCipheredPayload;

        // receipt info. will be empty string for iOS
        public string receipt;

		// Helper method to construct a new Product from JSON
		public static Product productFromJson(Json json)
		{
			Product p = new Product();
			try
			{
				p.name         = json["name"].string_value();
				p.id           = json["id"].string_value();
				p.type         = json["type"].string_value().StartsWith("non") ? Type.NON_CONSUMABLE : Type.CONSUMABLE;
				p.title        = json["title"].string_value();
				p.description  = json["desc"].string_value();
				p.price        = json["price"].string_value();
				p.priceValue   = json["priceValue"].float_value();
				p.currencyCode = json["currencyCode"].string_value();
				if (json.object_items().ContainsKey("receiptCipheredPayload"))
					p.receiptCipheredPayload = json["receiptCipheredPayload"].string_value();
				if (json.object_items().ContainsKey("receipt"))
					p.receipt = json["receipt"].string_value();
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				Debug.Log("Json: " + json.dump());
			}
			return p;
		}

		// Helper method to construct an array of products from JSON
		public static Product[] productsFromJson(List<Json> jsons)
		{
			Product[] products = new Product[jsons.Count];
			for(int i = 0; i < jsons.Count; ++i)
			{
				products[i] = Product.productFromJson(jsons[i]);
			}
			return products;
		}
	}

	[Serializable]
	public class IAP : PluginBase<IAP>
	{
		public List<ProductDescription> iOSProducts;

		public string androidKey;		
		public List<ProductDescription> androidProducts;

		[Serializable]
		public class Callbacks
		{
			[Serializable]
			public class BoolEvent : UnityEvent<bool> {}
			[Serializable]
			public class BoolStringEvent : UnityEvent<bool, string> {}
			[Serializable]
			public class ProductEvent : UnityEvent<Product> {}
			[Serializable]
			public class ProductStringEvent : UnityEvent<Product, string> {}
			[Serializable]
			public class ProductArrayEvent : UnityEvent<Product[]> {}
			[Serializable]
			public class StringEvent : UnityEvent<string> {}
	
			public BoolEvent          onInitialized           = null;
			public ProductEvent       onSuccess               = null;
			public ProductStringEvent onFailure               = null;
			public ProductEvent       onCanceled              = null;
			public ProductEvent       onRestored              = null;
			public ProductArrayEvent  onProductRequestSuccess = null;
			public StringEvent        onProductRequestFailure = null;
			public BoolStringEvent    onRestoreComplete       = null;

			Callbacks()
			{
				if (null == onInitialized)
					onInitialized = new BoolEvent();
				if (null == onSuccess)
					onSuccess = new ProductEvent();
				if (null == onFailure)
					onFailure = new ProductStringEvent();
				if (null == onCanceled)
					onCanceled = new ProductEvent();
				if (null == onRestored)
					onRestored = new ProductEvent();
				if (null == onProductRequestSuccess)
					onProductRequestSuccess = new ProductArrayEvent();
				if (null == onProductRequestFailure)
					onProductRequestFailure = new StringEvent();
				if (null == onRestoreComplete)
					onRestoreComplete = new BoolStringEvent();
			}
		};

		public Callbacks callbacks;

		// delegate signature for callbacks from SDKBOX runtime.
		public delegate void CallbackIAPDelegate(string method, string json);

		[MonoPInvokeCallback(typeof(CallbackIAPDelegate))]
		public static void sdkboxIAPCallback(string method, string jsonString)
		{
			if (null != _this)
			{
				var iap = (_this as IAP);
				iap.handleCallback(method, jsonString);
			}
			else
			{
				Debug.Log("Missed callback " + method + " => " + jsonString);
			}
		}

		private void handleCallback(string method, string jsonString)
		{
			Json json = Json.parse(jsonString);
			if (json.is_null())
			{
				Debug.LogError("Failed to parse JSON callback payload");
				throw new System.ArgumentException("Invalid JSON payload");
			}

			switch (method)
			{
				case "onInitialized":
					if (callbacks.onInitialized != null)
					{
						callbacks.onInitialized.Invoke(json["status"].bool_value());
					}
					break;
				case "onSuccess":
					if (callbacks.onSuccess != null)
					{
						callbacks.onSuccess.Invoke(Product.productFromJson(json["product"]));
					}
					break;
				case "onFailure":
					if (callbacks.onFailure != null)
					{
						callbacks.onFailure.Invoke(Product.productFromJson(json["product"]), json["message"].string_value());
					}
					break;
				case "onCanceled":
					if (callbacks.onCanceled != null)
					{
						callbacks.onCanceled.Invoke(Product.productFromJson(json["product"]));
					}
					break;
				case "onRestored":
					if (callbacks.onRestored != null)
					{
					callbacks.onRestored.Invoke(Product.productFromJson(json["product"]));
					}
					break;
				case "onProductRequestSuccess":
					if (callbacks.onProductRequestSuccess != null)
					{
						callbacks.onProductRequestSuccess.Invoke(Product.productsFromJson(json["products"].array_items()));
					}
					break;
				case "onProductRequestFailure":
					if (callbacks.onProductRequestFailure != null)
					{
						callbacks.onProductRequestFailure.Invoke(json["message"].string_value());
					}
					break;
				case "onRestoreComplete":
					if (callbacks.onRestoreComplete != null)
					{
						callbacks.onRestoreComplete.Invoke(json["status"].bool_value(), json["message"].string_value());
					}
					break;

				default:
					throw new System.ArgumentException("Unknown callback type: " + method);
			}
		}

		private string buildConfiguration()
		{
			Json config = newJsonObject();
			Json cur;

			cur = config;
			cur["ios"]   = newJsonObject(); cur = cur["ios"];
			cur["iap"]   = newJsonObject(); cur = cur["iap"];
			cur["items"] = newJsonObject(); cur = cur["items"];
			foreach (var p in iOSProducts)
			{
				Json j = newJsonObject();
				j["id"] = new Json(p.id);
				j["type"] = new Json(p.consumable ? "consumable" : "non_consumable");
				cur[p.name] = j;
			}

			cur = config;
			cur["android"] = newJsonObject(); cur = cur["android"];
			cur["iap"]     = newJsonObject(); cur = cur["iap"];
			cur["key"]     = new Json(androidKey);
			cur["items"]   = newJsonObject(); cur = cur["items"];
			foreach (var p in androidProducts)
			{
				Json j = newJsonObject();
				j["id"] = new Json(p.id);
				j["type"] = new Json(p.consumable ? "consumable" : "non_consumable");
				cur[p.name] = j;
			}

			return config.dump();
		}

		protected override void init()
		{
			Debug.Log("SDKBOX IAP starting.");

			SDKBOX.Instance.init(); // reference the SDKBOX singleton to ensure shared init.

			#if !UNITY_EDITOR
			var config = buildConfiguration();
			Debug.Log("configuration: " + config);

			#if UNITY_ANDROID
			IAP._player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
			AndroidJavaObject activity = IAP._player.GetStatic<AndroidJavaObject>("currentActivity");
			activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
			{
				// call IAP::init()
				sdkbox_iap_init(config);
				sdkbox_iap_set_unity_callback(sdkboxIAPCallback);
				sdkbox_iap_enableUserSideVerification(true);
				Debug.Log("SDKBOX IAP Initialized.");
			}));
			#else
			// call IAP::init()
			sdkbox_iap_init(config);
			sdkbox_iap_set_unity_callback(sdkboxIAPCallback);
			sdkbox_iap_enableUserSideVerification(true);
			Debug.Log("SDKBOX IAP Initialized.");
			#endif
			#endif // !UNITY_EDITOR
		}

		public void purchase(string name)
		{
			#if !UNITY_EDITOR
			#if UNITY_ANDROID
			AndroidJavaObject activity = IAP._player.GetStatic<AndroidJavaObject>("currentActivity");
			activity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
				sdkbox_iap_purchase(name);
			}));
			#else
			sdkbox_iap_purchase(name);
			#endif
			#endif // !UNITY_EDITOR
		}

		public void refresh()
		{
			#if !UNITY_EDITOR
			#if UNITY_ANDROID
			AndroidJavaObject activity = IAP._player.GetStatic<AndroidJavaObject>("currentActivity");
			activity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
				sdkbox_iap_refresh();
			}));
			#else
			sdkbox_iap_refresh();
			#endif
			#endif // !UNITY_EDITOR
		}

		public void restore()
		{
			#if !UNITY_EDITOR
			#if UNITY_ANDROID
			AndroidJavaObject activity = IAP._player.GetStatic<AndroidJavaObject>("currentActivity");
			activity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
				sdkbox_iap_restore();
			}));
			#else
			sdkbox_iap_restore();
			#endif
			#endif // !UNITY_EDITOR
		}

		#if UNITY_IOS
		[DllImport("__Internal")]
		#else
		[DllImport("iap")]
		#endif
		public static extern void sdkbox_iap_init(string jsonconfig);
		
		#if UNITY_IOS
		[DllImport("__Internal")]
		#else
		[DllImport("iap")]
		#endif
		public static extern void sdkbox_iap_purchase(string name);

		#if UNITY_IOS
		[DllImport("__Internal")]
		#else
		[DllImport("iap")]
		#endif	
		private static extern void sdkbox_iap_refresh();
		
		#if UNITY_IOS
		[DllImport("__Internal")]
		#else
		[DllImport("iap")]
		#endif	
		private static extern void sdkbox_iap_restore();
	
		#if UNITY_IOS
		[DllImport("__Internal")]
		#else
		[DllImport("iap")]
		#endif
		public static extern void sdkbox_iap_set_unity_callback(CallbackIAPDelegate callback);

		#if UNITY_IOS
		[DllImport("__Internal")]
		#else
		[DllImport("iap")]
		#endif
		public static extern void sdkbox_iap_enableUserSideVerification(bool enabled);
	}
}
