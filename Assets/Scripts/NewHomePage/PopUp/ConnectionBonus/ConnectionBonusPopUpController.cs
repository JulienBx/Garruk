using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class ConnectionBonusPopUpController : MonoBehaviour 
{
	public void reset(int bonus)
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingConnectionBonusPopUp.getReference(0)+bonus.ToString()+ WordingConnectionBonusPopUp.getReference(1);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text =  WordingConnectionBonusPopUp.getReference(2);
		gameObject.transform.FindChild ("Button").GetComponent<ConnectionBonusPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<ConnectionBonusPopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
	}
	public void exitPopUp()
	{
		SoundController.instance.playSound(8);
		NewHomePageController.instance.hideConnectionBonusPopUp ();
	}
}

