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
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Confirmez vous la suppression de l'équipe "+deckName + ".\n\n Attention cette action est irréversible ! \n Vous perdrez définitivement votre deck mais vous conserverez vos unités.\n";
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Confirmer";
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

