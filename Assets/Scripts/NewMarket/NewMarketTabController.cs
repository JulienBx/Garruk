using UnityEngine;
using TMPro;

public class NewMarketTabController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		NewMarketController.instance.selectATabHandler(base.getId());	
	}
}

