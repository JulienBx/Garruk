using UnityEngine;
using TMPro;

public class NewMarketSortButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		NewMarketController.instance.sortButtonHandler (base.getId());	
	}
}