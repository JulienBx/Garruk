using UnityEngine;
using TMPro;

public class NewSkillBookSkillTypeFilterTitleController : TextToHoverController	
{	
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingFilters.getReference(33),WordingFilters.getReference(34));
	}
}

