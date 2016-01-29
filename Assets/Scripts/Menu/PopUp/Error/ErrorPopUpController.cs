using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class ErrorPopUpController : MonoBehaviour 
{
	public void reset(string error)
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = error;
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingErrorPopUp.getReference(0);
		gameObject.transform.FindChild ("Button").GetComponent<ErrorPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<ErrorPopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
	}
	public void exitPopUp()
	{
		MenuController.instance.hideErrorPopUp ();
	}
}

