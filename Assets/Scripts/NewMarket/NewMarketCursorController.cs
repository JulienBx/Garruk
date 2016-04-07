using UnityEngine;
using TMPro;

public class NewMarketCursorController : SpriteButtonController 
{
	void OnMouseDrag()
	{
		NewMarketController.instance.moveCursors (base.getId ());	
	}
	public override void OnMouseDown()
	{
		NewMarketController.instance.startSlidingCursors();
	}
	public override void OnMouseUp()
	{
		NewMarketController.instance.endSlidingCursors();
	}
	public override void setHoveredState()
	{
	}
}

