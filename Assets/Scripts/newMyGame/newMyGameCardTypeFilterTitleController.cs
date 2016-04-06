using UnityEngine;
using TMPro;

public class newMyGameCardTypeFilterTitleController : TextToHoverController	
{	
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(false, true,"middle", this.gameObject,WordingFilters.getReference(13),WordingFilters.getReference(14));
	}
}

