using UnityEngine;
using TMPro;

public class NewStoreBackButtonController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		NewStoreController.instance.backToPacksHandler();
	}
}