using UnityEngine;
using TMPro;

public class newMyGameValueFilterValueController : TextToHoverController	
{	
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingFilters.getReference(29),WordingFilters.getReference(30));
	}
}

