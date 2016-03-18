using UnityEngine;
using System.Collections;

public class FullDeskController : MonoBehaviour {

	public Transform parent;

	void Start() {

	}

	public void Open(bool b){
		if (b) {
			gameObject.transform.SetParent (GetComponentInParent<Canvas> ().transform);
		} else {
			gameObject.transform.SetParent (parent);
			GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
		}

//		if (isFirstTime) {
//			gameObject.transform.SetParent (GetComponentInParent<Canvas> ().transform);
//			isFirstTime = false;
//		}
		gameObject.SetActive (b);
	}
}
