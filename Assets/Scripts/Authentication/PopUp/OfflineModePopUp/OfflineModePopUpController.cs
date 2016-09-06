using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class OfflineModePopUpController : MonoBehaviour 
{

	public void reset()
	{
		this.computeLabels();
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild ("Button").GetComponent<OfflineModePopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild("CloseButton").GetComponent<OfflineModePopUpCloseButtonController>().reset();
	}
	public void computeLabels()
	{
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingOfflineModePopUp.getReference(0);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingOfflineModePopUp.getReference(1);
	}
	public void resize()
	{
	}
	public void exitPopUp()
	{
		SoundController.instance.playSound(8);
		AuthenticationController.instance.hideOfflineModePopUp();
	}
}

