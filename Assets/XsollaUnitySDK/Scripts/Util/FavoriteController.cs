using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FavoriteController : MonoBehaviour {

	public Toggle toggle;
	public Text text;

	// Use this for initialization
	void Start () {
		ChangeValue (false);
		toggle.onValueChanged.AddListener ((b) => ChangeValue(b));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeValue(bool b){
		if (toggle.isOn)
			text.text = "";
		else
			text.text = "";
	}

}
