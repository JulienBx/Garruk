using UnityEngine;
using TMPro;

public class NewFocusedCardExperienceTitleController : TextToHoverController 
{
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingFocusedCard.getReference(13),WordingFocusedCard.getReference(14));
	}
}

