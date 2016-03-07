using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class AuthenticationMessagePopUpController : MonoBehaviour 
{

	private int labelId;

	public void reset(int labelId)
	{
		this.labelId=labelId;
		this.computeLabels();
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild ("Button").GetComponent<AuthenticationMessagePopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild("CloseButton").GetComponent<AuthenticationMessagePopUpCloseButtonController>().reset();
	}
	public void computeLabels()
	{
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingAuthenticationMessagePopUp.getReference(labelId);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingAuthenticationMessagePopUp.getReference(0);
	}
	public void resize()
	{
	}
	public void exitPopUp()
	{
		SoundController.instance.playSound(8);
		AuthenticationController.instance.displayLoginPopUp();
		AuthenticationController.instance.hideAuthenticationMessagePopUp();
	}
}

