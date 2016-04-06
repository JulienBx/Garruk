using UnityEngine;
using TMPro;

public class newMyGameValueSkillSearchTitleController : TextToHoverController	
{	
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(false, true,"middle", this.gameObject,WordingFilters.getReference(23),WordingFilters.getReference(24));
	}
}

