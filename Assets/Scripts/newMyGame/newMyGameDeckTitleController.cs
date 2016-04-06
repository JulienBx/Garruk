using UnityEngine;
using TMPro;

public class newMyGameDeckTitleController : TextToHoverController	
{	
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(false, true,"left", this.gameObject,WordingDeck.getReference(22),WordingDeck.getReference(23));
	}
}

