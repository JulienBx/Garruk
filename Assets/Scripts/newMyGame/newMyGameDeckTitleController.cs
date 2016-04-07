using UnityEngine;
using TMPro;

public class newMyGameDeckTitleController : TextToHoverController	
{	
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingDeck.getReference(22),WordingDeck.getReference(23));
	}
}

