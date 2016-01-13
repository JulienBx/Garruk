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
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Félicications !!\n\nVous avez débloqué la faction :\n\n" + name;
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Continuer";
		gameObject.transform.FindChild ("Button").GetComponent<NewCardTypePopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<NewCardTypePopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
	}
	public void exitPopUp()
	{
		MenuController.instance.hideNewCardTypePopUp ();
	}
}

