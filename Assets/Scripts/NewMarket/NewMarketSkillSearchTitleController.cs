using UnityEngine;
using TMPro;

public class NewMarketSkillSearchTitleController : TextToHoverController	
{	
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingFilters.getReference(23),WordingFilters.getReference(24));
	}
}

