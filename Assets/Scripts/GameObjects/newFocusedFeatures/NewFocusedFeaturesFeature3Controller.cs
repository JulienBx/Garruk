using UnityEngine;
using TMPro;

public class NewFocusedFeaturesFeature3Controller : TextToHoverController 
{
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingFocusedCard.getReference(39),WordingFocusedCard.getReference(40));
	}
}

