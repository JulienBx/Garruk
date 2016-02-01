using UnityEngine;
using TMPro;

public class NewMarketCursorController : SpriteButtonController 
{
	void OnMouseDrag()
	{
		NewMarketController.instance.moveCursors (base.getId ());	
	}
	void OnMouseDown()
	{
		NewMarketController.instance.startSlidingCursors();
	}
	void OnMouseUp()
	{
		NewMarketController.instance.endSlidingCursors();
	}
	public override void setHoveredState()
	{
	}
}

