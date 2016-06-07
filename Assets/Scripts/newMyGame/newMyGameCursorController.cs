using UnityEngine;
using TMPro;

public class newMyGameCursorController : SpriteButtonController 
{
	void OnMouseDrag()
	{
		if(HelpController.instance.canAccess(-1))
		{
			newMyGameController.instance.moveCursors (base.getId ());
		}
	}
	public override void OnMouseDown()
	{
		if(HelpController.instance.canAccess(-1))
		{
			newMyGameController.instance.startSlidingCursors();
		}
	}
	public override void OnMouseUp()
	{
		if(HelpController.instance.canAccess(-1))
		{
			newMyGameController.instance.endSlidingCursors();
		}
	}
	public override void setHoveredState()
	{
	}
}

