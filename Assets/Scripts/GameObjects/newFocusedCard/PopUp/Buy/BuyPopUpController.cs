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
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text ="Confirmer le recrutement de l'unité (coûte "+price+ " cristaux)";
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Confirmer";
		gameObject.transform.FindChild ("Button").GetComponent<BuyPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<BuyPopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
	}
	public void buyHandler()
	{
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().buyCardHandler ();
	}
	public void exitPopUp()
	{
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().hideBuyPopUp ();
	}
	
}

