using UnityEngine;
using TMPro;

public class NewFocusedCardTitleController : TextToHoverController 
{
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingFocusedCard.getReference(19),WordingFocusedCard.getReference(20));
	}
}

