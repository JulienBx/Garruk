using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class WonPackPopUpController : MonoBehaviour 
{
	public void reset()
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingWonPackPopUp.getReference(0);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text =  WordingWonPackPopUp.getReference(1);
		gameObject.transform.FindChild ("Button").GetComponent<WonPackPopUpConfirmButtonController> ().reset ();
	}
	public void resize()
	{
	}
	public void exitPopUp()
	{
		SoundController.instance.playSound(8);
		NewHomePageController.instance.hideWonPackPopUp ();
	}
}

