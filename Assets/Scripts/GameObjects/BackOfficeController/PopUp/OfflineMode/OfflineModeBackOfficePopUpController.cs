﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class OfflineModeBackOfficePopUpController : MonoBehaviour 
{
	public void reset(int idMessage)
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingOfflineModeBackOfficePopUp.getReference(idMessage);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingOfflineModeBackOfficePopUp.getReference(0);
		gameObject.transform.FindChild ("Button").GetComponent<OfflineModeBackOfficePopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("Button2").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingOfflineModeBackOfficePopUp.getReference(1);
		gameObject.transform.FindChild ("Button2").GetComponent<OfflineModeBackOfficePopUpCancelButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<OfflineModeBackOfficePopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
	}
	public void reconnectHandler()
	{
		SoundController.instance.playSound(8);
		StartCoroutine (this.reconnect ());
	}
	public IEnumerator reconnect()
	{
		BackOfficeController.instance.displayLoadingScreen ();
		yield return StartCoroutine (ApplicationModel.player.syncData ());
		BackOfficeController.instance.hideLoadingScreen ();
		if (ApplicationModel.player.IsOnline) {
			MenuController.instance.refreshMenuObject ();
			PhotonController.instance.connectToPhoton ();	
		} else {
			BackOfficeController.instance.displayDetectOfflinePopUp ();
		}
		BackOfficeController.instance.hideOfflineModePopUp ();
	}
	public void exitPopUp()
	{
		SoundController.instance.playSound(8);
		BackOfficeController.instance.hideOfflineModePopUp ();
	}
}

