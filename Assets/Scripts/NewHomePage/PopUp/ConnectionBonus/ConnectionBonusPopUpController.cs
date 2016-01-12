using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class ConnectionBonusPopUpController : MonoBehaviour 
{
	public void reset(int bonus)
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Première connection de la journée, vous gagnez "+bonus.ToString()+" cristaux";
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Confirmer";
		gameObject.transform.FindChild ("Button").GetComponent<ConnectionBonusPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<ConnectionBonusPopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
	}
	public void exitPopUp()
	{
		NewHomePageController.instance.hideConnectionBonusPopUp ();
	}
}

