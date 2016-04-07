using UnityEngine;
using TMPro;

public class NewMarketValueFilterTitleController : TextToHoverController	
{	
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingFilters.getReference(21),WordingFilters.getReference(22));
	}
}

