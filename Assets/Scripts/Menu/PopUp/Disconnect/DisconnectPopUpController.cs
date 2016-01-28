using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class DisconnectPopUpController : MonoBehaviour 
{
	public void reset()
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Souhaitez-vous quitter le jeu ?";
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Confirmer";
		gameObject.transform.FindChild ("Button").GetComponent<DisconnectPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("Button2").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Annuler";
		gameObject.transform.FindChild ("Button2").GetComponent<DisconnectPopUpCancelButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<DisconnectPopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
	}
	public void quitGameHandler()
	{
		MenuController.instance.logOutLink();
	}
	public void exitPopUp()
	{
		MenuController.instance.hideDisconnectedPopUp ();
	}
}

