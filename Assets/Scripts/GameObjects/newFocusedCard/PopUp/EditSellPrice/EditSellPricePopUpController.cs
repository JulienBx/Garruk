using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class EditSellPricePopUpController : MonoBehaviour 
{
	public void reset(int price)
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingEditSellPricePopUp.getReference(0);
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = "";
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingEditSellPricePopUp.getReference(1);
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setText (price.ToString());
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setFocused();
		gameObject.transform.FindChild ("Button").GetComponent<EditSellPricePopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<EditSellPricePopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().resize ();
	}
	public void setError(string error)
	{
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = error;
	}
	public void editSellPriceHandler()
	{
		SoundController.instance.playSound(8);
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().editSellPriceCardHandler();
	}
	public void exitPopUp()
	{
		SoundController.instance.playSound(8);
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().hideEditSellPricePopUp();
	}
	public string getFirstInput()
	{
		return gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().getText ();
	}
}

