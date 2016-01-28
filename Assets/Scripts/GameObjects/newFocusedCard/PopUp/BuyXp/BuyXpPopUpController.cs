using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class BuyXpPopUpController : MonoBehaviour 
{
	public void reset(int price)
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Confirmer la montée de niveau de l'unité (coûte " + price + " cristaux)";
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Confirmer";
		gameObject.transform.FindChild ("Button").GetComponent<BuyXpPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<BuyXpPopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
	}
	public void buyXpHandler()
	{
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().buyXpCardHandler ();
	}
	public void exitPopUp()
	{
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().hideBuyXpPopUp ();
	}
	
}

