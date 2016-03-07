using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class ChangePasswordPopUpController : MonoBehaviour 
{
	public void reset()
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingChangePasswordPopUp.getReference(0);
		gameObject.transform.FindChild ("Title2").GetComponent<TextMeshPro> ().text = WordingChangePasswordPopUp.getReference(1);
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = "";
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingChangePasswordPopUp.getReference(2);
		gameObject.transform.FindChild ("Input").GetComponent<InputPasswordGuiController> ().setText ("");
		gameObject.transform.FindChild ("Input").GetComponent<InputPasswordGuiController> ().setFocused();
		gameObject.transform.FindChild ("Input2").GetComponent<InputPasswordGuiController> ().setText ("");
		gameObject.transform.FindChild ("Button").GetComponent<ChangePasswordPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<ChangePasswordPopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
		gameObject.transform.FindChild ("Input").GetComponent<InputPasswordGuiController> ().resize ();
		gameObject.transform.FindChild ("Input2").GetComponent<InputPasswordGuiController> ().resize ();
	}
	public void setError(string error)
	{
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = error;
	}
	public void checkPasswordHandler()
	{
		SoundController.instance.playSound(8);
		NewProfileController.instance.editPasswordHandler();
	}
	public void exitPopUp()
	{
		SoundController.instance.playSound(8);
		NewProfileController.instance.hideChangePasswordPopUp ();
	}
	public string getFirstPassword()
	{
		return gameObject.transform.FindChild ("Input").GetComponent<InputPasswordGuiController> ().getText ();
	}
	public string getSecondPassword()
	{
		return gameObject.transform.FindChild ("Input2").GetComponent<InputPasswordGuiController> ().getText ();
	}
}

