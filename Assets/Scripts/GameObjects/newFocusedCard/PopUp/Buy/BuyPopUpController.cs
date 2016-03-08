using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class BuyPopUpController : MonoBehaviour 
{
	public void reset(int price)
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text =WordingBuyPopUp.getReference(0)+price+ WordingBuyPopUp.getReference(1);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingBuyPopUp.getReference(2);
		gameObject.transform.FindChild ("Button").GetComponent<BuyPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<BuyPopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
	}
	public void buyHandler()
	{
		SoundController.instance.playSound(8);
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().buyCardHandler ();
	}
	public void exitPopUp()
	{
		SoundController.instance.playSound(8);
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().hideBuyPopUp ();
	}
	
}

