using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class SearchUsersPopUpButtonController: SimpleButtonController
{
	void OnMouseDown()
	{
		gameObject.transform.parent.GetComponent<SearchUsersPopUpController> ().exitPopUp ();
	}
}

