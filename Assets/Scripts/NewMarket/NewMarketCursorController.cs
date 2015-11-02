using UnityEngine;
using TMPro;

public class NewMarketCursorController : SpriteButtonController 
{
	void OnMouseDrag()
	{
		NewMarketController.instance.moveCursors (base.getId ());	
	}
	public override void setHoveredState()
	{
	}
}

