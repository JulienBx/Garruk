using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class DeleteDeckPopUpController : MonoBehaviour 
{
	public void reset(string deckName)
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingDeleteDeckPopUp.getReference(0)+deckName + WordingDeleteDeckPopUp.getReference(1);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingDeleteDeckPopUp.getReference(2);
		gameObject.transform.FindChild ("Button").GetComponent<DeleteDeckPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<DeleteDeckPopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
	}
	public void deleteDeckHandler()
	{
		newMyGameController.instance.deleteDeckHandler();
	}
	public void exitPopUp()
	{
		newMyGameController.instance.hideDeleteDeckPopUp ();
	}
}

