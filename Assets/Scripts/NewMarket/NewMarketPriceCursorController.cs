using UnityEngine;
using TMPro;

public class NewMarketPriceCursorController : SpriteButtonController 
{
	void OnMouseDrag()
	{
		NewMarketController.instance.moveMinMaxCursor (base.getId ());	
	}
	public override void setHoveredState()
	{
	}
}

