using UnityEngine;
using TMPro;

public class newMyGameCursorController : SpriteButtonController 
{
	void OnMouseDrag()
	{
		if(!HelpController.instance.getIsTutorialLaunched())
		{
			newMyGameController.instance.moveCursors (base.getId ());
		}
	}
	public override void OnMouseDown()
	{
		if(!HelpController.instance.getIsTutorialLaunched())
		{
			newMyGameController.instance.startSlidingCursors();
		}
	}
	public override void OnMouseUp()
	{
		if(!HelpController.instance.getIsTutorialLaunched())
		{
			newMyGameController.instance.endSlidingCursors();
		}
	}
	public override void setHoveredState()
	{
	}
}

