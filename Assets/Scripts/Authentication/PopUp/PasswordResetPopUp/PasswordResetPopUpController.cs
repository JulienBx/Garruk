using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class PasswordResetPopUpController : MonoBehaviour 
{
	public void reset()
	{
		this.computeLabels();
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild ("Button").GetComponent<PasswordResetPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild("CloseButton").GetComponent<PasswordResetPopUpCloseButtonController>().reset();
	}
	public void computeLabels()
	{
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingPasswordResetPopUp.getReference(0);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingPasswordResetPopUp.getReference(1);
	}
	public void resize()
	{
	}
	public void exitPopUp()
	{
		AuthenticationController.instance.displayLoginPopUp();
	}
}

