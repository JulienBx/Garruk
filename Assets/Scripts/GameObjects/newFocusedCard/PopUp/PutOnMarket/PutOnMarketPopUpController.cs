using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class PutOnMarketPopUpController : MonoBehaviour 
{
	public void reset()
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Choisir le prix en vente de la carte sur le marché";
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = "";
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Confirmer";
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setText ("");
		gameObject.transform.FindChild ("Button").GetComponent<PutOnMarketPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<PutOnMarketPopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().resize ();
	}
	public void setError(string error)
	{
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = error;
	}
	public void putOnMarketHandler()
	{
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().putOnMarketCardHandler();
	}
	public void exitPopUp()
	{
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().hidePutOnMarketPopUp ();
	}
	public string getFirstInput()
	{
		return gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().getText ();
	}
}

