using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class ProfileMessagePopUpController : MonoBehaviour 
{
	private int labelId;

	public void reset(int labelId)
	{
		this.labelId=labelId;
		this.computeLabels();
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild ("Button").GetComponent<ProfileMessagePopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild("CloseButton").GetComponent<ProfileMessagePopUpCloseButtonController>().reset();
	}
	public void computeLabels()
	{
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingProfileMessagePopUp.getReference(labelId);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingProfileMessagePopUp.getReference(0);
	}
	public void resize()
	{
	}
	public void exitPopUp()
	{
		NewProfileController.instance.hideMessagePopUp();
	}
}

