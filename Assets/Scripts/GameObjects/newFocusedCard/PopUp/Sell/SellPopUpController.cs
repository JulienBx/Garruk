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
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Confirmer le bannissement de l'unité (rapporte " + price + " cristaux). \n\n Attention cette action est irréversible ! \n Vous perdrez définitivement votre unité.\n";
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Confirmer";
		gameObject.transform.FindChild ("Button").GetComponent<SellPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<SellPopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
	}
	public void sellHandler()
	{
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().sellCardHandler ();
	}
	public void exitPopUp()
	{
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().hideSellPopUp ();
	}

}

