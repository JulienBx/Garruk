using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopItemToggleController : MonoBehaviour {

	public Toggle toggle;
	
	// Update is called once per frame
	void Update () {
		if (toggle != null && toggle.isOn && Input.GetButtonDown ("Fire1"))
			toggle.isOn = false;
	}
}
