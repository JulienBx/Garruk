using UnityEngine;
using TMPro;

public class newMyGameCursorController : SpriteButtonController 
{
	void OnMouseDrag()
	{
		newMyGameController.instance.moveCursors (base.getId ());	
	}
	void OnMouseDown()
	{
		newMyGameController.instance.startSlidingCursors();
	}
	void OnMouseUp()
	{
		newMyGameController.instance.endSlidingCursors();
	}
	public override void setHoveredState()
	{
	}
}

