using UnityEngine;
using TMPro;

public class newMyGameValueFilterTitleController : TextToHoverController	
{	
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(false, true,"middle", this.gameObject,WordingFilters.getReference(21),WordingFilters.getReference(22));
	}
}

