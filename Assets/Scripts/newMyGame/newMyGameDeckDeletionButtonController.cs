using UnityEngine;
using TMPro;

public class newMyGameDeckDeletionButtonController : SimpleButtonController
{	
	public override void OnMouseDown()
	{
		newMyGameController.instance.displayDeleteDeckPopUp ();
	}
}

