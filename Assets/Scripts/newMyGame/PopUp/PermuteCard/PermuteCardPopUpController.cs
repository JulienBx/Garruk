using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class PermuteCardPopUpController : MonoBehaviour 
{
	private int deckPosition;

	public void reset(int position)
	{
		this.deckPosition=position;
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingPermuteCardPopUp.getReference(0);
		gameObject.transform.FindChild ("PermuteButton").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingPermuteCardPopUp.getReference(1);
		gameObject.transform.FindChild ("CancelButton").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingPermuteCardPopUp.getReference(2);
		gameObject.transform.FindChild ("PermuteButton").GetComponent<PermuteCardPopUpPermuteButtonController> ().reset ();
		gameObject.transform.FindChild ("CancelButton").GetComponent<PermuteCardPopUpCancelButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<PermuteCardPopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
	}
	public void permuteCardHandler()
	{
		SoundController.instance.playSound(8);
		newMyGameController.instance.permuteCardHandler(this.deckPosition);	
	}
	public void exitPopUp()
	{
		SoundController.instance.playSound(8);
		newMyGameController.instance.hidePermuteCardPopUp();
	}
}

