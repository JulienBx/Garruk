using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class SellPopUpController : MonoBehaviour 
{
	public void reset(int price)
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingSellPopUp.getReference(0) + price + WordingSellPopUp.getReference(1);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingSellPopUp.getReference(2);
		gameObject.transform.FindChild ("Button").GetComponent<SellPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<SellPopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
	}
	public void sellHandler()
	{
		SoundController.instance.playSound(8);
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().sellCardHandler ();
	}
	public void exitPopUp()
	{
		SoundController.instance.playSound(8);
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().hideSellPopUp ();
	}

}

