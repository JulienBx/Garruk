using UnityEngine;
using TMPro;

public class newMyGameCursorController : SpriteButtonController 
{
	void OnMouseDrag()
	{
		newMyGameController.instance.moveCursors (base.getId ());	
	}
	public override void setHoveredState()
	{
	}
}

