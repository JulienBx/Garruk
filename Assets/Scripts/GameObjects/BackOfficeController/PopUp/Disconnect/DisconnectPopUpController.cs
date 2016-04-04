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
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingDisconnectPopUp.getReference(0);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingDisconnectPopUp.getReference(1);
		gameObject.transform.FindChild ("Button").GetComponent<DisconnectPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("Button2").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingDisconnectPopUp.getReference(2);
		gameObject.transform.FindChild ("Button2").GetComponent<DisconnectPopUpQuitButtonController> ().reset ();
		gameObject.transform.FindChild ("Button3").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingDisconnectPopUp.getReference(3);
		gameObject.transform.FindChild ("Button3").GetComponent<DisconnectPopUpCancelButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<DisconnectPopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
	}
	public void quitHandler()
	{
		SoundController.instance.playSound(8);
		Application.Quit();	
	}
	public void logOutHandler()
	{
		SoundController.instance.playSound(8);
		MenuController.instance.logOutLink();
	}
	public void exitPopUp()
	{
		SoundController.instance.playSound(8);
		BackOfficeController.instance.hideDisconnectedPopUp ();
	}
}

