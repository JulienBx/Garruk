using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class NewCardTypePopUpController : MonoBehaviour 
{
	public void reset(string name)
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingNewCardTypePopUp.getReference(0) + name;
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingNewCardTypePopUp.getReference(1);
		gameObject.transform.FindChild ("Button").GetComponent<NewCardTypePopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<NewCardTypePopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
	}
	public void exitPopUp()
	{
		BackOfficeController.instance.hideNewCardTypePopUp ();
	}
}