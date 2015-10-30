using UnityEngine;
using TMPro;

public class newMyGameDeckSelectionButtonController : SimpleButtonController
{	
	public override void OnMouseDown()
	{
		newMyGameController.instance.displayDeckList();	
	}
	public override void setIsHovered(bool value)
	{
		base.setIsHovered (value);
		newMyGameController.instance.mouseOnSelectDeckButton(value);
	}
}

