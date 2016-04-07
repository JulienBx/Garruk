using UnityEngine;
using TMPro;

public class NewCardLifeController : TextToHoverController 
{
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingFocusedCard.getReference(37),WordingFocusedCard.getReference(38));
	}
}

