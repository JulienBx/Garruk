using UnityEngine;
using TMPro;

public class NewSkillBookSkillAvailabilityFilterTitleController : TextToHoverController	
{	
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingFilters.getReference(35),WordingFilters.getReference(36));
	}
}

