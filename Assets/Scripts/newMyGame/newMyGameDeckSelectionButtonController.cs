using UnityEngine;
using TMPro;

public class newMyGameDeckSelectionButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		newMyGameController.instance.displayDeckList();	
	}
	public override void setIsHovered(bool value)
	{
		base.setIsHovered (value);
		newMyGameController.instance.mouseOnSelectDeckButton(value);
	}
}

