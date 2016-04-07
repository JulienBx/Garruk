using UnityEngine;
using TMPro;

public class NewMarketPriceFilterTitleController : TextToHoverController	
{	
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingFilters.getReference(31),WordingFilters.getReference(32));
	}
}

