using UnityEngine;
using TMPro;

public class NewStoreBackButtonController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		NewStoreController.instance.backToPacksHandler();
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingStore.getReference(11),WordingStore.getReference(12));
	}
}