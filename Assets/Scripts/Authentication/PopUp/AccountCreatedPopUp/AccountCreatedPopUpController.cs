using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class AccountCreatedPopUpController : MonoBehaviour 
{
	public void reset()
	{
		this.computeLabels();
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild ("Button").GetComponent<AccountCreatedPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild("CloseButton").GetComponent<AccountCreatedPopUpCloseButtonController>().reset();
	}
	public void computeLabels()
	{
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingAccountCreatedPopUp.getReference(0);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingAccountCreatedPopUp.getReference(1);
	}
	public void resize()
	{
	}
	public void exitPopUp()
	{
		AuthenticationController.instance.displayLoginPopUp();
	}
}

