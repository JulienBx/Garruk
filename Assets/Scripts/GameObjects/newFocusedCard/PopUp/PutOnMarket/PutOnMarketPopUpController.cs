using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class PutOnMarketPopUpController : MonoBehaviour 
{
	public void reset(int price)
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingPutOnMarketPopUp.getReference(0) + price.ToString() + WordingPutOnMarketPopUp.getReference(1);
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = "";
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingPutOnMarketPopUp.getReference(2);
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setText ("");
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setFocused();
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
		SoundController.instance.playSound(8);
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().putOnMarketCardHandler();
	}
	public void exitPopUp()
	{
		SoundController.instance.playSound(8);
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().hidePutOnMarketPopUp ();
	}
	public string getFirstInput()
	{
		return gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().getText ();
	}
}

