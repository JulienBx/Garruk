using UnityEngine;
using TMPro;

public class newMyGameDeckSelectionButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		newMyGameController.instance.displayDeckList();	
	}
	public override void OnMouseOver()
	{
		if(base.getIsActive())
		{
			if(!base.getIsSelected())
			{
				if(!base.getIsHovered())
				{
					if(!ApplicationDesignRules.isMobileScreen)
					{
						this.setHoveredState();
					}
					this.setIsHovered(true);
					newMyGameController.instance.mouseOnSelectDeckButton(true);
				}
			}
		}
	}
	public override void OnMouseExit()
	{
		if(base.getIsActive())
		{	
			if(!base.getIsSelected())
			{
				if(base.getIsHovered())
				{
					if(!ApplicationDesignRules.isMobileScreen)
					{
						this.setInitialState();
					}
					this.setIsHovered(false);
					newMyGameController.instance.mouseOnSelectDeckButton(false);
				}
			}
		}
	}
}

