using UnityEngine;
using TMPro;

public class NewHomePageBuyPackButtonController : SimpleButtonController
{	
	public override void OnMouseDown()
	{
		NewHomePageController.instance.buyPackHandler();	
	}
	public override void setIsHovered(bool value)
	{
		base.setIsHovered (value);
		NewHomePageController.instance.mouseOnBuyPackButton(value);
	}
}

