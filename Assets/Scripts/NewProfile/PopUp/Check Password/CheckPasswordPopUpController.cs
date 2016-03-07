using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class CheckPasswordPopUpController : MonoBehaviour 
{
	public void reset()
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingCheckPasswordPopUp.getReference(0);
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = "";
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingCheckPasswordPopUp.getReference(1);
		gameObject.transform.FindChild ("Input").GetComponent<InputPasswordGuiController> ().setText ("");
		gameObject.transform.FindChild ("Input").GetComponent<InputPasswordGuiController> ().setFocused();
		gameObject.transform.FindChild ("Button").GetComponent<CheckPasswordPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<CheckPasswordPopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
		gameObject.transform.FindChild ("Input").GetComponent<InputPasswordGuiController> ().resize ();
	}
	public void setError(string error)
	{
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = error;
	}
	public void checkPasswordHandler()
	{
		SoundController.instance.playSound(8);
		NewProfileController.instance.checkPasswordHandler();
	}
	public void exitPopUp()
	{
		SoundController.instance.playSound(8);
		NewProfileController.instance.hideCheckPasswordPopUp ();
	}
	public string getPassword()
	{
		return gameObject.transform.FindChild ("Input").GetComponent<InputPasswordGuiController> ().getText ();
	}
}

