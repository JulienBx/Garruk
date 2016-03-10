using UnityEngine;
using System.Collections;
using System;

public class OpenPaystation : MonoBehaviour {

	public event Action<string> TryOpenPaystation;

	public string token;
	public bool isSandbox;

	public void InitPaystation()
	{
		if (token != null && !"".Equals(token)) {
			string url;
			if(!isSandbox){
				url = "https://secure.xsolla.com/paystation2/?access_token=" + token;
			} else {
				url = "https://sandbox-secure.xsolla.com/paystation2/?access_token=" + token;
			}
			Application.OpenURL (url);
			OnOpenPaystation(url);
		}
	}

	private void OnOpenPaystation(string url)
	{
		if (TryOpenPaystation != null)
			TryOpenPaystation (url);
	}

}
