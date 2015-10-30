using UnityEngine;
using TMPro;

public class NewHomePageDeckSelectionButtonController : SimpleButtonController
{	
	public override void OnMouseDown()
	{
		NewHomePageController.instance.deckSelectionButtonHandler();	
	}
	public override void setIsHovered(bool value)
	{
		base.setIsHovered (value);
		NewHomePageController.instance.mouseOnSelectDeckButton(value);
	}
}

