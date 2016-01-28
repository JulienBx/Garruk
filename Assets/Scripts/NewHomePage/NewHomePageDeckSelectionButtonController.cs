using UnityEngine;
using TMPro;

public class NewHomePageDeckSelectionButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		NewHomePageController.instance.deckSelectionButtonHandler();	
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
					NewHomePageController.instance.mouseOnSelectDeckButton(true);
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
					NewHomePageController.instance.mouseOnSelectDeckButton(false);
				}
			}
		}
	}
}

