using UnityEngine;
using TMPro;

public class newMyGameCursorController : SpriteButtonController 
{
	void OnMouseDrag()
	{
		if(!TutorialObjectController.instance.getIsTutorialDisplayed())
		{
			newMyGameController.instance.moveCursors (base.getId ());
		}
	}
	void OnMouseDown()
	{
		if(!TutorialObjectController.instance.getIsTutorialDisplayed())
		{
			newMyGameController.instance.startSlidingCursors();
		}
	}
	new void OnMouseUp()
	{
		if(!TutorialObjectController.instance.getIsTutorialDisplayed())
		{
			newMyGameController.instance.endSlidingCursors();
		}
	}
	public override void setHoveredState()
	{
	}
}

