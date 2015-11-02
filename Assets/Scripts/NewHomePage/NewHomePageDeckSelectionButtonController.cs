using UnityEngine;
using TMPro;

public class NewHomePageDeckSelectionButtonController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		NewHomePageController.instance.deckSelectionButtonHandler();	
	}
	public override void setIsHovered(bool value)
	{
		base.setIsHovered (value);
		NewHomePageController.instance.mouseOnSelectDeckButton(value);
	}
}

