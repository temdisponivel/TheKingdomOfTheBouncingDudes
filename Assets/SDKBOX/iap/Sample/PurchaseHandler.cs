using UnityEngine;
using System.Collections;
using Sdkbox;

public class PurchaseHandler : MonoBehaviour 
{
	private Sdkbox.IAP _iap;

	// Use this for initialization
	void Start() 
	{
		_iap = FindObjectOfType<Sdkbox.IAP>();
		if (_iap == null)
		{
			Debug.Log("Failed to find IAP instance");
		}
	}

	public void Purchase(string item) 
	{
		if (_iap != null)
		{
			Debug.Log("About to purchase " + item);
			_iap.purchase(item);
		}
	}

	public void Refresh() 
	{
		if (_iap != null)
		{
			Debug.Log("About to refresh");
			_iap.refresh();
		}
	}

	public void Restore() 
	{
		if (_iap != null)
		{
			Debug.Log("About to restore");
			_iap.restore();
		}
	}

	//
	// Event Handlers
	//

	public void onInitialized(bool status)
	{
		Debug.Log("PurchaseHandler.onInitialized " + status);
	}

	public void onSuccess(Product product)
	{
		Debug.Log("PurchaseHandler.onSuccess: " + product.name);
	}

	public void onFailure(Product product, string message)
	{
		Debug.Log("PurchaseHandler.onFailure " + message);
	}

	public void onCanceled(Product product)
	{
		Debug.Log("PurchaseHandler.onCanceled product: " + product.name);
	}

	public void onRestored(Product product)
	{
		Debug.Log("PurchaseHandler.onRestored: " + product.name);
	}

	public void onProductRequestSuccess(Product[] products)
	{
		foreach (var p in products)
		{
			Debug.Log("Product: " + p.name + " price: " + p.price);
		}
	}

	public void onProductRequestFailure(string message)
	{
		Debug.Log("PurchaseHandler.onProductRequestFailure: " + message);
	}

	public void onRestoreComplete(string message)
	{
		Debug.Log("PurchaseHandler.onRestoreComplete: " + message);
	}
}
