using UnityEngine;
using TMPro;

public class NewFocusedCardAttackController : TextToHoverController 
{
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingFocusedCard.getReference(35),WordingFocusedCard.getReference(36));
	}
}

