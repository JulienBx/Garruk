﻿using UnityEngine;
using TMPro;

public class newMyGameCardTypeFilterTitleController : TextToHoverController	
{	
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingFilters.getReference(13),WordingFilters.getReference(14));
	}
}

