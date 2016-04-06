using UnityEngine;
using TMPro;

public class newMyGameValueFilterIconController : TextToHoverController	
{	
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(true, false,"", this.gameObject,WordingFilters.getReference(15+base.getId()*2),WordingFilters.getReference(16+base.getId()*2));
	}
}

