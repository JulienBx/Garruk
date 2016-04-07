using UnityEngine;
using TMPro;

public class NewMarketValueFilterIconController : TextToHoverController	
{	
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingFilters.getReference(15+base.getId()*2),WordingFilters.getReference(16+base.getId()*2));
	}
}

